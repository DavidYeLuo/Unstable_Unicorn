# Project
Recreating the board game Unstable Unicorn in Unity for fun

# Software
Unity 6000.0.33f1  
[Mirror](https://mirror-networking.com/) or websocket
# Tech Stack
- Engine: Unity 6000.0.33f1
- Networking (planned):
  - Mirror or WebSocket-based solution

Data Format: JSON (for card definitions and configuration)

# Project Structure
```
Assets/
├── Data/
│   └── CardsJSON.json
│      - Stripped card data adapted from:
│        https://github.com/geniegeist/unstable-unicorns
│
├── Prefab/
│   └── CardTemplate
│      - Base prefab used to instantiate and visually configure cards
│
├── Scenes/
│   └── SecondPrototype
│      - Current main development scene
│
├── Scripts/
│   └── (All gameplay, logic, and system scripts)
│
├── Resources/
│   └── Card Sprites
│      - Artwork used to render cards
│      - Sourced from geniegeist’s repository
```


# Progress
- Implemented
  - Deck System
    - Card deck creation
    - Shuffling
    - Drawing cards
  - Frontend Display
    - Card GameObjects instantiated from prefabs
    - Visual representation of cards using sprites and data
- In Progress
  - Game State Management
    - Designing a clear state flow (e.g., setup, draw phase, action phase, end turn)
  - Defining how transitions between states are handled
- Planned
  - Card Effects System
    - Implementing effect logic for different card types
    - Creating a scalable framework to support complex and chained effects
  - Multiplayer Support
    - Synchronizing game state across clients
    - Player actions, turns, and card interactions via Mirror or WebSockets
