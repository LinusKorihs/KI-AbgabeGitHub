import socket
import csv
import json
from RewardSystem import calculate_reward
from Brain import TetrisBrain

# Setup (Test)
action_space = 5
state_space = 1000
ai = TetrisBrain(action_space, state_space)

import json

# Load Values from JSON
training_state_file = "Training_state.json"

def save_training_state(total_reward):
    """
    Save the training state to a JSON file.
    """
    state = {
        "total_reward": total_reward
    }
    with open(training_state_file, "w") as file:
        json.dump(state, file)
    #print("[PYTHON] Training state saved.")

def load_training_state():
    """
    Load the training state from a JSON file.
    """
    try:
        with open(training_state_file, "r") as file:
            content = file.read().strip()
            if not content:
                return 0, 0, 0
            state = json.loads(content)
            #print("[PYTHON] Training state loaded:", state)
            return state.get("total_reward", 0)
    except FileNotFoundError:
        return 0, 0, 0
    except json.JSONDecodeError as e:
        return 0, 0, 0
    
# Global variables
total_reward = load_training_state()
average_action_value, average_action_counter = 0, 0
episode_reward = 0
total_lines_cleared = 0
new_q_modular, new_q_value, new_q_counter = 0, 0, 0

# Episode management
def save_episode_number(episode_number):
    with open("episode_number.txt", "w") as file:
        file.write(str(episode_number))

def load_episode_number():
    try:
        with open("episode_number.txt", "r") as file:
            return int(file.read())
    except FileNotFoundError:
        return 0

episode_number = load_episode_number()

# Logging
def log_training_data(episode_number, episode_reward, total_reward, action, new_q, lines_cleared):
    """
    Logs the training data to a CSV file.
    """
    file_path = "training_log.csv"
    header = ['Episode', 'Episode-Reward', 'Total-Reward', 'Action', 'Q-value', 'LinesCleared']

    # Check if the file exists
    try:
        with open(file_path, 'x') as file:
            writer = csv.DictWriter(file, fieldnames=header)
            writer.writeheader()
    except FileExistsError:
        pass

    # Append the data
    with open(file_path, 'a') as file:
        writer = csv.DictWriter(file, fieldnames=header)
        writer.writerow({
            'Episode': episode_number,
            'Episode-Reward': episode_reward,
            'Total-Reward': total_reward,
            'Action': action,
            'Q-value': new_q,
            'LinesCleared': lines_cleared
        })

# Data parsing
def parse_client_data(data_str):
    """Splits the received data into game state string and result dictionary."""
    game_state_str, result_info = data_str.split("|")
    result_dict = {}
    game_state = {}

    for entry in game_state_str.split(";"):
        if ":" in entry:
            key, value = entry.split(":", 1)
            if key == "linesCleared":  # Speichere linesCleared explizit
                result_dict[key] = int(value)
            elif value.lower() in ["true", "false"]:
                result_dict[key] = value.lower() == "true"
            else:
                game_state[key] = value

    for entry in result_info.split(","):
        if ":" in entry:
            key, value = entry.split(":", 1)
            result_dict[key] = int(value) if value.isdigit() else value.lower() == "true"

    return game_state, result_dict

def encode_state(game_state):
    return hash(game_state) % ai.state_space

def decode_action(action_id):
    return str(action_id)

# Game logic
def handle_game_over():
    global episode_reward, total_reward, average_action_value, average_action_counter, episode_number, total_lines_cleared, new_q_value, new_q_counter, new_q_modular

    gameOver = 1
    current_state = encode_state("gameOver_dummy_before")
    next_state = encode_state("gameOver_dummy_after")
    action = None  # No action for Game Over
    reward = calculate_reward({}, None, gameOver, lines_cleared=0)
    episode_reward += reward
    total_reward += reward

    # Update Q-value
    new_q_value += ai.update_q_value(current_state, action, reward, episode_reward, next_state)
    new_q_counter += 1
    new_q_modular = round(new_q_value / new_q_counter, 5)
    ai.decay_epsilon()

    total_reward = round(total_reward, 3)

    # Log data
    actionAVG = round(average_action_value / average_action_counter, 2) if average_action_counter > 0 else 0
    log_training_data(episode_number, round(episode_reward, 3), total_reward, actionAVG, new_q_modular, total_lines_cleared)
    print(f"[PYTHON] Episode {episode_number}, Episode Reward: {round(episode_reward,3)}, Total Reward: {round(total_reward,3)}, ActionAVG: {actionAVG}, Q-value: {new_q_modular}, Lines Cleared: {total_lines_cleared}")

    # Reset episode variables

    episode_reward = 0
    episode_number += 1
    save_episode_number(episode_number)
    save_training_state(total_reward)
    average_action_counter = 0
    average_action_value = 0
    total_lines_cleared = 0

    return "Game Over"

def handle_game_step(received_str):
    global episode_reward, total_reward, average_action_value, average_action_counter, total_lines_cleared, episode_number, new_q_value, new_q_counter, new_q_modular

    gameOver = 0
    game_state, result_info = parse_client_data(received_str)
    current_state = encode_state(game_state["gridBefore"])
    next_state = encode_state(game_state["gridAfter"])

    ai.adjust_q_bounds(episode_number)

    # Choose action
    action = ai.choose_action(current_state)
    average_action_value += action
    average_action_counter += 1

    # Simulate action
    next_state = (current_state + action) % ai.state_space

    # Reward logic
    lines_cleared = result_info.get("linesCleared", 0)
    reward = calculate_reward(result_info, action, gameOver, lines_cleared)
    episode_reward += reward
    total_reward += reward
    total_lines_cleared += lines_cleared
    #save_training_state(total_reward, average_action_value, average_action_counter)
    
    # Update Q-value
    new_q_value += ai.update_q_value(current_state, action, reward, episode_reward, next_state)
    new_q_counter += 1
    new_q_modular = round(new_q_value / new_q_counter, 5)
    #print(f"[PYTHON] New Q-value: {new_q_modular}")

    return decode_action(action)

# Server setup
def start_server():
    print(f"[PYTHON] Starting server on {HOST}:{PORT}...")
    while True:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server:
            server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            server.bind((HOST, PORT))
            server.listen()
            #print("[PYTHON] Waiting for client connection...")

            conn, addr = server.accept()
            with conn:
                #print(f"[PYTHON] Connected with {addr}")
                while True:
                    try:
                        data = conn.recv(1024)
                        if not data:
                            #print("[PYTHON] Client disconnected.")
                            break

                        received_str = data.decode()
                        #print(f"[PYTHON] Received: {received_str}")

                        if "gameOver:true|" in received_str and "gridBefore" not in received_str:
                            response = handle_game_over()
                            conn.sendall(response.encode())
                            break
                        else:
                            response = handle_game_step(received_str)
                            conn.sendall(response.encode())

                    except ConnectionResetError:
                        print("[PYTHON] Connection reset by client.")
                        break
                    except Exception as e:
                        print(f"[PYTHON] Error: {e}")
                        break

# Main
if __name__ == "__main__":
    HOST = '127.0.0.1'
    PORT = 65432
    start_server()