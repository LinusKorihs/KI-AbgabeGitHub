import json

_q_table = {}
_q_table_path = "q_table.json"

def load_q_table():
    global _q_table
    try:
        with open(_q_table_path, 'r') as f:
            _q_table = json.load(f)
        print(f"[QTable] Loaded Q-table from {_q_table_path}")
    except FileNotFoundError:
        print(f"[QTable] No Q-table found. Starting fresh.")
        _q_table = {}

def save_q_table():
    with open(_q_table_path, 'w') as f:
        json.dump(_q_table, f, indent=4, sort_keys=True)
    #print(f"[QTable] Saved Q-table to {_q_table_path}")

def get_q_table():
    return _q_table

def get_q_value(state, action):
    state = str(state)
    action = str(action)
    value = _q_table.get(state, {}).get(action, 0)  # Standardwert 0
    #print(f"[QTable] Get Q-value for state {state}, action {action}: {value}")
    return value

def set_q_value(state, action, value):
    state = str(state)
    action = str(action)
    if state not in _q_table:
        _q_table[state] = {}
    _q_table[state][action] = value

def print_q_table():
    #print("[QTable] Current Q-table:")
    for state, actions in _q_table.items():
        print(f"  State {state}: {actions}")

# Load once at import
load_q_table()

if __name__ == "__main__":
    print("Current Q-Table\n:")
    # First 10 states for brevity
    for i, (state, actions) in enumerate(_q_table.items()):
        if i >= 10:  # Limit to first 10 states for brevity
            break
        print(f"State {state}: {actions}")

