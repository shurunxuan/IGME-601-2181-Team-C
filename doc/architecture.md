#### Architecture Proposal
For the engine, we will be using Unity. This means that our architecture will need to connect to and build on top of Unity's existing architecture. 
Unity's architecture requires us to work in C# scripts that can be used as components and attached to Game Objects in each level. 
The editor is very well put to together and allows the linking of components and objects without requiring changing or accessing code.
Since we have such a nice modular component system to take advantage of, I believe it would be a mistake to not take full advantage of it.
I will now detail what our architecture might look like, to have it have it take advantage of those features. 
Generally, the idea is to make it so that all of the level design can be done in a data driven way without modifying the scripts themselves.
I will avoid details on things that are already handled by Unity, and will presume that the reader is familiar with Unity.

## Environment
Static, non-interactive, environmental objects do not require much thought as they are handled by Unity sufficiently already.
Objects with behaviors will need scripts that detail their behaviors.
As much as possible, these EnvironmentBehavior Scripts should be data driven, general purpose, and specific to a single behavior.
EnvironmentBehavior components should all implement the same abstract component that allows other parts of the code to send information to the behavior script.
Objects that detect a condition in the game and trigger a behavior, should also use the same class of script for such behaviors.
These should implement a parent EnvironmentDetector script that knows how to interface with EnvironmentBehavior scripts when connected in the editor.
Anything that could be considered to require detailed interfacing with the player, may require an additional script that denotes it as such, but it may be unnecesary.

## Guards
Guards should follow a pattern of movement that is predictable and involves simple paths or behaviors.
These movement patterns should use a single movement pattern script that the main AI script uses to determine how it should follow its path.
This pattern script should be data driven using handplaced pathfinding nodes.
Guards should detect directly via sight as well as pay attention to a SoundDetectionManager that tracks major sounds. These will be handled by two different detection scripts.
Sight detection will use a simple raycast that also considers the angle from the line of sight of the guard.
Sound will be kept track of by a manager that has a list of Sound events and what portions of the map these events effect.
Sound detection will check ask the manager what sounds it hears each frame and the guard will act based on sounds it hears and the directions they seem to come from.
Either detection will fill the alertness meter, if the alertness meter is filled and the guard has sight of the player, the main game manager will end the game.
The alertness meter will empty over time, but as it does the guard will wander in the direction it thinks the sound came from for a time.

## Player
Player has a PlayerControlComponent that looks at the keyboard input and acts as the hub for the Player's components to communicate.
It has a DroneMovementComponent that knows how to move based on the input given by the PlayerControlComponent.
It has an EnergyComponent that keeps track of how much energy the player has and has useful functions for manipulating the resource.
ToolComponents will implement the same abstract component that allows them to interface with the PlayerControlComponent more easily.
ToolComponents will be used to handle all of the functionality of the drone other than movement and will implement their own logic to fulfill their purpose.
The modularity of ToolComponents will allow us to standardize them and make them easy to add and remove and/or allow the player to customize their loadout.
The actual customization will be available through a menu that is not accessable while a level is in progress.

## Managers
A number of static manager classes will be used.
These managers will need to handle score, victory conditions, loss conditions, menus, sound based detection, saving, loading, sound the user hears, heads up display, and possibly more things.
For these managers, we should seperate their responsabilities when possible, but one manager could probably fulfill multiple roles if they are closely tied.
For now, we should start with a score manager that knows how much top secret info you've gathered, a menu manager that loads menus on request, a game state transition manager, a sound detection manager and a heads up display manager.
