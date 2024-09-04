# Moldus Shroomastered Edition

## About the Game
The game is a Dungeon Crawler in Unity. To start it, just run the `.exe` file.

## Old Moldus
Moldus was an idea created during the Matfyz Summer Game Jam. The game was quite broken and did not feature procedural generation. I did half of the programming on the game, mainly everything that did not feature enemy AI and pathfinding. I decided to make a new version of the game due to the fact that the idea and assets already exist, so I don't have to create new ones because I can't draw. I would like to give my thanks to Jaromir Prochazka for letting me use his sprites and for creating three new ones.

## How to Play
The game is a Dungeon Crawler where you need to search a procedurally generated level for the exit. The level is filled with enemies of three types, and you must defeat them using either your starting flamethrower or by switching infections. This can be achieved by killing enough enemies of a certain type and then pressing one of the `1`, `2`, `3` keys that are associated with the enemy type.

## Controls
- **WASD** for movement
- **Left Mouse Button** for attack
- The `1`, `2`, `3` buttons on the number row switch the player's infection if the corresponding infection has enough points.
- **Esc** shows the pause menu

## Code

### Unity Objects

#### Player
Has a rigid body, `SpriteRenderer`, collider, animator, and scripts: `PlayerMovementController.cs`, `PlayerStatsController.cs`, `PlayerShootingController.cs`, `HealthController.cs`. This represents the player and moves and shoots according to the player's input. It has a flamethrower as a basic attack and can switch between weapons by collecting enough points and changing infections. The `PlayerStatsController.cs` updates the `ShootingController` for that. It is all configurable.

#### Flame
An object representing the attack of the flamethrower that deals damage and adds visual effects. Contains collider, `SpriteRenderer`, and animator.

#### Explosion
A purely visual object for the explosion of a bomb, with an animator, `SpriteRenderer`, and an audio source.

#### Pathfinding
An object that manages scanning the map for enemy pathfinding. I am using the Astar Pathfinding Project https://arongranberg.com/astar/.

#### Start and End
The start is where the player spawns, and the end is the point where the level ends, and the score increments. Both have a `SpriteRenderer`; the End has the `EndOfLevelController.cs`.

#### PointsManager
Contains all the UI and point management scripts: `EnemyKillCountController.cs`, `HealthTextController.cs`, `InGameMenuController.cs`, `ScoreTrackerController.cs`, `SaveLoadController.cs`.

#### Enemies
There are three types of enemies. Each has a rigid body, `SpriteRenderer`, collider, animator, and scripts. All enemies contain the basic behavior in `BasicEnemy.cs` and `HealthController.cs`. They then contain the corresponding script for their color, meaning the other scripts in the Enemy folder. Mainly, it adds a type of attack. They also contain pathfinding from the Astar Pathfinding Project, which is used for pathfinding. By scanning the map, these enemies can find the shortest paths to their target.

#### Spawners
Has `SpriteRenderer`, collider, `HealthController.cs`, and `SpawnerController.cs`. The object spawns enemies around itself and can be destroyed by attacking it.

#### Bullet
Contains a `SpriteRenderer`, rigid body, collider, and a `BulletController.cs` script that manages it by checking for collisions.

#### Main Dungeon Generator and Content Generator
The Main Dungeon Generator just contains the `RoomGenerator.cs` script and calls it on Awake, and other objects call it for restarts. `DungeonContentGenerator` contains just the `DungeonContentGenerator` script; it could have been in the same object.

#### Camera Helper
This object is designed so that the Cinemachine Camera follows it. The `CameraController.cs` script makes it move in the direction the player is looking.

#### Helping Hierarchical Objects like Enemies
Just ensures that the Unity hierarchy is always comprehensible and helps with erasing certain objects.

#### MusicManager
Just plays music on a loop.

#### Background
Just a giant sprite to cover the default Unity background.

### Scripts

#### Game

##### Bullet
- **FlameController.cs**:  
  Checks if something entered the flame trigger and then calls the `TakeDamage` function.
- **ExplosionController.cs**:  
  Controls the visual object for the explosion, making the object exist long enough to run the animation and then destroys itself.
- **BombController.cs**:  
  Waits a certain amount of time, then checks for all the objects in the collider and calls a `TakeDamage` function.
- **BulletController.cs**:  
  Checks if anything collided with the bullet to make it take damage. Configures the bullet.

###### BulletData
Contains the scriptable object and the variants to have the same bullet objects with different properties like damage and range.

##### Camera
- **CameraController.cs**:  
  Moves the helping camera object to always be between the mouse and the player.

##### Entity

###### HealthController.cs
Allows objects to call the `TakeDamage` function.

###### Enemy
Contains the following scripts:
- **BasicEnemy.cs**:  
  Handles configuration and behavior for the basic enemy. This script:
  - Determines enemy behavior in relation to pathfinding.
  - Checks the distance and visibility of the player to decide on actions.
- **IEnemy.cs**:  
  An interface that all enemies inherit from, providing a consistent structure for enemy behavior and interactions.
- **GreenEnemyController.cs**:  
  Gives the green enemy a specific attack of shooting a projectile. It is identical to `PurpleEnemyController.cs`, but their bullets and sprites are different.
- **PurpleEnemyController.cs**:  
  Gives the purple enemy a specific attack of shooting a projectile. It is identical to `GreenEnemyController.cs`, but their bullets and sprites are different.
- **RedEnemyController.cs**:  
  Overrides the `EnemyBehaviour` function so it can run away from the bomb it places and sets up its attack as `PlantBomb`.

###### Player
- **PlayerMovementController.cs**:  
  Manages the movement of the player relative to the input. Uses the `InputSystem` package, but due to confusion, mostly uses the default.
- **PlayerStatsController.cs**:  
  Counts how many enemies of a specific type have been killed and manages the transformations (infections).
- **PlayerShootingController.cs**:  
  Manages the flamethrower and bullet shooting, including their spawning, initialization, and direction. Also checks to turn off the flamethrower.

###### Spawner
- **SpawnerController.cs**:  
  Contains the `SpawnerController.cs` script that spawns enemies around the spawner. It checks if some of them died via event so it can spawn more.

##### Management
Scripts that manage scores and points. `ScoreTrackerController.cs` manages the incrementation of score when ending the level. `EnemyKillCountController.cs` manages points earned after killing an enemy.

##### Procedural Generation
The procedural generation scripts are separated into many folders. The `Abstracts` contain an abstract class, and the `Algorithms` are just static. The main generation is done using the Binary Space Partitioning algorithm and then either randomly or semi-randomly placing tiles, spawners, and others, or calculated like the start and end of the level. I used this video as a tutorial for the Binary Space Partitioning Algorithms: [YouTube](https://www.youtube.com/watch?v=-QOCX6SVFsk&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v).
- **AbstractDungeonGenerator.cs**:  
  Ensures the dungeon generator has some basic fields and functions. This script is pretty obsolete.
- **BinarySpacePartitioningAlgorithms.cs**:  
  The main algorithm takes parameters and generates a bunch of `BoundsInt` that represent the rooms. The `BinarySpacePartitioning` function does this.
- **DungeonContentGeneratorAlgorithms.cs**:  
  Contains two algorithms: `PlaceCoordinatesCircularPatternInRoom` generates a bunch of coordinates for spawners in a circular pattern, and `GetTwoRoomsFurthestFromEachOther` finds two rooms furthest from each other to decide where the start and end are.
- **DungeonContentGenerator.cs**:  
  Destroys all the enemies and spawners in a reset and manages the placement of spawners and the pathfinding.
- **RoomConnectAlgorithm.cs**:  
  A static class with an algorithm that, using room centers, generates paths between them by searching for the closest center to the current one in an expanding maze. It ignores the fact that it cuts through other rooms, which I believe makes more interesting level layouts.
- **RoomGenerator.cs**:  
  Contains the `RunProceduralGeneration` function that erases the old dungeon and generates a new one. It is the main script for generation that calls all the algorithms and then visualizes them.
- **TileMapVisualizer.cs**:  
  Contains the tilemaps, can clear them, get coordinates of a tile converted to a real Unity coordinate, and visualize the whole level if given coordinates of the floor. It will even paint the passable walls for perspective.
- **WallGenerator.cs**:  
  Adds a wall tile next to all the floor tiles and even checks for corners.

##### Structures
Contains scripts for structures, in this case just the `EndOf
