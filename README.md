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
✅ Test-Driven Development (TDD) Overview of All Scripts
This document provides a brief overview of each script and its responsibilities in the game. It can be used as a reference for debugging, testing, and future improvements.

📜 Core Game Systems
1️⃣ GameManager.cs
Initializes and manages all core game systems (UnitFactory, TurnManager, EnergySystem, SpawnManager).
Calls Initialize() on all managers at game start.
Triggers OnGameStart event.

2️⃣ TurnManager.cs
Manages turn-based gameplay.
Handles turn switching and triggers OnTurnSwitch event.
Ensures land roles switch properly.

3️⃣ SpawnManager.cs
Listens for OnInputReceived to spawn units at the correct location.
Deducts energy from the correct player before spawning.
Listens for OnTurnReset to remove all units and reset the field.

4️⃣ EventHolder.cs
Central event system for communication between components.
Manages events like OnGameStart, OnTurnSwitch, OnEnergyRequest, OnBallPickedUp, and OnUnitDataRequest.

📜 Unit Management & Pooling

5️⃣ UnitFactory.cs
Manages unit spawning using object pooling (PoolManager).
Stores unit prefabs and data.
Provides a method to get active units.

6️⃣ PoolManager.cs
Generic object pooling system for performance optimization.
Recycles units instead of destroying them.
Supports retrieving and returning active objects.

📜 Units & Gameplay Mechanics

7️⃣ Unit.cs (Base Class)
Parent class for all units (Attacker, Defender).
Handles activation logic after spawn.
Controls the isActive state.

8️⃣ Attacker.cs
Handles movement and chasing the ball.
Uses AttackerInteraction for collision logic.
Moves straight if no ball is present.
Moves toward the goal when carrying the ball.

9️⃣ Defender.cs
Uses DefenderStateBehaviour to manage state transitions (Standby, Chasing, Inactive).
Moves toward attackers if detected.
Ignores collisions when inactive.

🔟 DefenderStateBehaviour.cs
Controls defender behavior during different states.
Handles transitioning between Standby → Chasing → Inactive.
Moves defenders back to their spawn point when inactive.

1️⃣1️⃣ AttackerInteraction.cs
Handles attacker-specific interactions.
Detects if the attacker is caught and triggers PassBallToNearestAttacker().

1️⃣2️⃣ DefenderInteraction.cs
Handles defender-specific interactions.
Detects if an attacker has the ball and triggers a chase.

📜 Ball & Passing Mechanics

1️⃣3️⃣ BallController.cs
Handles ball interactions (PickUp, PassToAttacker).
Moves toward the nearest attacker when passed.
Listens for OnRequestBallStatus to provide ball information.

📜 Energy & UI

1️⃣4️⃣ EnergySystem.cs
Stores and regenerates player energy over time.
Listens for OnEnergyRequest to deduct energy.

1️⃣5️⃣ EnergyUI.cs
Updates the UI bars based on current energy levels.
Uses EnergyBarSegment to control individual UI elements.

1️⃣6️⃣ EnergyBarSegment.cs
Controls the visual fill and transparency of an energy bar segment.

📜 Input & Land Management

1️⃣7️⃣ InputHandler.cs
Detects user input and sends OnInputReceived event.
Uses Raycast to detect the land field.

1️⃣8️⃣ Land.cs
Stores LandType (Attacker/Defender) and playerID.
Sends OnInputReceived when clicked.
Switches LandType when the turn changes.

📜 Utility & Interfaces

1️⃣9️⃣ GlobalHolder.cs
Stores global enums (LandType, UnitType, DefenderState).
Stores global interfaces (IUnit, IInteractable, IInputReader, IMovable).

2️⃣0️⃣ MovementHandler.cs
Handles movement logic for units.
Ensures rotation only happens on the Y-axis.

2️⃣1️⃣ InteractionWrapper.cs
Attaches IInteractable to GameObjects.
Fixes TryGetComponent<IInteractable> issue.
