def calculate_reward(info, actVar, gameOver, lines_cleared):
    reward = 0

    reward += 0.35 # Base reward for taking any action

    if actVar == 2:
        reward -= 0.35  # Small penalty for rotation
    
    if actVar == 3:
        reward += 0.3 # Small bonus for dropping

    if actVar == 4:
        reward -= 0.1 # Small penalty for doing nothing

    # Reward for clearing lines
    reward += lines_cleared * 100
    if lines_cleared >= 2:
        reward += (lines_cleared - 1) * 2  # Extra bonus
    if lines_cleared > 0:    
        print(f"[PYTHON] Lines cleared: {lines_cleared}, Reward: {reward}")

       # Penalty for blocked moves
    if info.get("moveBlocked", False):
        reward -= 0.25  # Penalty for attempting a blocked move

    # Penalty for losing
    if gameOver == 1:
        reward -= 4.5
    
    return round(reward, 4)
