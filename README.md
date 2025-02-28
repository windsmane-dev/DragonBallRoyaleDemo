# DragonBallRoyaleDemo
Clash Royale Meets Dragons Meets Balls

# Caution:- 
# This is a WIP project. Mechanics as requested are not all implemented yet. 
# Currently using Whiteboxing for visuals. 

# Pending List:- 
- Animation
- Replace Models
- Add VFX
- Scoring System
- Penalty Game
- AR module

# This game uses a SOLID compliant, event driven architecture, made to be scalable in the future.

TDD:- 
The aim is to keep the codebase as clean and modular as time permits. 
Gamemanager is the entry point, it initializes and spawns all managers to remove Human Error. 
Only scripts that are already on the scene are:- 
- Land.CS (Responsible for responding to touch using ISelectable Interface), and responding to event calls to get position inside of the land or its orientation. 
- Fence.CS (Responsible for Triggering Return of attacker, Uses IInteractable Interface)
- Goal.CS (Responsible for triggering event to switch turns and triggering event for increasing score)
- GameManager.CS As metioned above
- InputHandler.CS (Responsible for reading inputs and raising events related to input)

GLOBALHOLDER.CS is a global script that holds the static event handler, all interface and enum declarations. 
EventHandler is where all events are cetnralized. 

 Full TDD will be available here as a readme when the project is completed. (WIP) 
