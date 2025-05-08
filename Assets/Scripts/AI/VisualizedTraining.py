import matplotlib.pyplot as plt
import csv

def read_training_data(file_path):
    """
    Reads the training data from a CSV file.
    """
    episodes = []
    episode_rewards = []
    total_rewards = []
    actionsAVG = []
    new_q = []
    lines_cleared = []

    with open(file_path, 'r') as file:
        reader = csv.DictReader(file)
        if reader.fieldnames is None:
            raise ValueError("CSV file is missing a header. Please add a header: 'Episode,Episode-Reward,Total-Reward,ActionAVG,Q-Value'.")

        for row in reader:
            episodes.append(int(row['Episode']))
            episode_rewards.append(float(row['Episode-Reward']))
            total_rewards.append(float(row['Total-Reward']))
            actionsAVG.append(float(row['ActionAVG']))
            new_q.append(float(row['Q-Value']))
            lines_cleared.append(int(row['LinesCleared']))

    return episodes, episode_rewards, total_rewards, actionsAVG, new_q, lines_cleared

def plot_total_rewards(episodes, total_rewards):
    """
    Plots the total rewards over episodes.
    """
    plt.figure(figsize=(10, 6))
    plt.plot(episodes, total_rewards, label="Total Rewards", color="blue")
    plt.xlabel("Episode")
    plt.ylabel("Total Reward")
    plt.title("Total Rewards Over Episodes")
    plt.legend()
    plt.grid()
    plt.show()

def plot_episode_rewards(episodes, episode_rewards):
    """
    Plots the episode rewards over episodes.
    """
    plt.figure(figsize=(10, 6))
    plt.plot(episodes, episode_rewards, label="Episode Rewards", color="orange")
    plt.xlabel("Episode")
    plt.ylabel("Episode Reward")
    plt.title("Episode Rewards Over Episodes")
    plt.legend()
    plt.grid()
    plt.show()

def plot_actionsAVG(episodes, actionsAVG):
    """
    Plots the actionsAVG over episodes.
    """
    plt.figure(figsize=(10, 6))
    plt.plot(episodes, actionsAVG, label="ActionAVG", color="green", linestyle="-", marker="o", markersize=4)
    plt.xlabel("Episode")
    plt.ylabel("ActionAVG ID")
    plt.title("ActionsAVG Over Episodes")
    plt.legend()
    plt.grid()
    plt.show()

def plot_new_q(episodes, new_q):
    """
    Plots the new Q-Values over episodes.
    """
    plt.figure(figsize=(10, 6))
    plt.plot(episodes, new_q, label="New Q-Value", color="red")
    plt.xlabel("Episode")
    plt.ylabel("New Q-Value")
    plt.title("New Q-Values Over Episodes")
    plt.legend()
    plt.grid()
    plt.show()

def plot_lines_cleared(episodes, lines_cleared):
    """
    Plots the number of lines cleared over episodes.
    """
    plt.figure(figsize=(10, 6))
    plt.plot(episodes, lines_cleared, label="Lines Cleared", color="purple", linestyle="-", marker="o", markersize=4)
    plt.xlabel("Episode")
    plt.ylabel("Lines Cleared")
    plt.title("Lines Cleared Over Episodes")
    plt.legend()
    plt.grid()
    plt.show()

if __name__ == "__main__":
    # Path to the CSV file
    file_path = "training_log.csv"

    # Read the data
    episodes, episode_rewards, total_rewards, actionsAVG, new_q, lines_cleared = read_training_data(file_path)

    # Plot the data
    plot_episode_rewards(episodes, episode_rewards)
    plot_total_rewards(episodes, total_rewards)
    plot_actionsAVG(episodes, actionsAVG)
    plot_new_q(episodes, new_q)
    plot_lines_cleared(episodes, lines_cleared)