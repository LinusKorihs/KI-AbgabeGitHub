import random
from QTableManager import get_q_value, set_q_value, save_q_table

class TetrisBrain:
    def __init__(self, action_space, state_space, alpha=0.225, gamma=0.9, epsilon=0.3):
        self.alpha = alpha      # Learning rate, typically between 0.1 and 0.5. Lower values mean slower learning, but more stable.
        self.gamma = gamma      # Discount factor, typically between 0.9 and 0.99. Higher values mean the agent will consider future rewards more.
        self.epsilon = epsilon  # Exploration rate, typically between 0.1 and 0.3. Higher values mean more exploration.
        self.action_space = action_space + 1 # +1 for the "do nothing" action
        self.state_space = state_space
        self.max_q = 100
        self.min_q = -50

    def choose_action(self, state):
        """
        Choose an action based on the epsilon-greedy strategy.
        """
        if random.uniform(0, 1) < self.epsilon:
            return random.choice(range(self.action_space))  # Exploration
        else:
            # Exploitation: Choose the action with the highest Q-value
            q_values = [get_q_value(state, a) for a in range(self.action_space)]
            #print(f"[TetrisBrain] State: {state}, Q-Values: {q_values}")
            return q_values.index(max(q_values))

    def update_q_value(self, state, action, reward, episode_reward, next_state):
        """
        Update the Q-Value using the Q-Learning formula. Returns for logging purposes.
        """
        current_q = get_q_value(state, action)
        #print(f"[TetrisBrain] Current Q-value for state {state}, action {action}: {current_q}")
        #print(f"[TetrisBrain] Current Q-value for state {state}, action {action}: {current_q}")
        episode_reward_factor = 0.1 * episode_reward if episode_reward > 0 else 0
        max_next_q = max([get_q_value(next_state, a) for a in range(self.action_space)])
        max_next_q = max(0, max_next_q)  # Ensure max_next_q is non-negative
        new_q = current_q + self.alpha * ((reward + episode_reward_factor) + self.gamma * max_next_q - current_q)
        
        # Ensure the new Q-value is within the defined bounds
        new_q = max(self.min_q, min(self.max_q, new_q))
        #print(f"[TetrisBrain] New Q-value for state {state}, action {action}: {new_q} (Reward: {reward}, Episode Reward: {episode_reward})")
        
        set_q_value(state, action, new_q)
        #print(f"[TetrisBrain] Updated Q-value for state {state}, action {action}: {new_q}")
        save_q_table()

        return new_q
    
    def decay_epsilon(self, min_epsilon=0.05, decay_rate=0.999):
        self.epsilon = max(min_epsilon, self.epsilon * decay_rate)
        #print(f"[TetrisBrain] Epsilon decayed to: {self.epsilon}")

    def adjust_q_bounds(self, episode_number):
        """
        Dynamically adjust the max_q and min_q values based on the episode number.
        """
        # Example logic to adjust Q bounds based on episode number
        self.max_q = 50 + episode_number * 0.05  # Max value increases slowly
        self.min_q = -25 - episode_number * 0.25  # Min value decreases slowly
        
        #print(f"[TetrisBrain] Adjusted Q bounds: max_q={self.max_q}, min_q={self.min_q}")
