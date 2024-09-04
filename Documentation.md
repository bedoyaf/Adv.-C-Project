# Moldus Shroomastered Edition


## About the game
The game is a Dungeon Crawler in Unity, to start it just run the .exe file.
## Old Moldus
Moldus was an idea created during the matfyz summer game jam, the game was quite broken and did not feature procedural generation. I did half of the programming on the game mainly everithing that did not feature enemy AI and pathfinding. I decided to make a new version of the game due to the fact that the idea and assets already exist so I dont have to create new ones because I cant draw. So I would give my thanks here to Jaromir Prochazka for letting me use his sprites and creating 3 new ones.
## How to play
The game is a Dungeon Crawler where you need to search a proceduraly generated level for the level exist, the level is filled with enemies of 3 types you must defeat them using either your starting flamethrower or by switching infections, that can be achieved by killing enaugh enemies of a certain type and then pressing one of the 1 2 3 keys that are asocieted with the enemy type. 
## Controls
WASD for movement
Left Mouse Butten for Attack
the 1 2 3 buttons on the number row switch players infection if the corresponding infection has enaugh points.
Esc shows the pause menu
## Code

### Unity Objects

#### Player
Has rigid body, spriteRenderer, collider, animator and scripts, PlayerMovementController.cs, PlayerStatsController.cs, PlayerShootingController.cs, HealthController.cs.
This represents represents the player and moves shoots according to the players input, it has a flamethrower as a basic attack and can switch between weapons by collecting enauigh points and changing infections. The PlayerStatsController.cs updates the shootingController for that. It is all configurable.
#### FLame
Just and object representing the attack of the flamethrower that deals damage and adds visual, contains collider sprite renderer and animator.
#### Explosion
Purely visual object for the explosion of a bomb, has animator and spriterenderer and an audio source
#### Pathfinding
An object that manages scanning the map for the enemy pathfinding, I am using the Astar Pathfinding Project.
#### Start and End
just the spot where the player spawns and the point where the level ends and the score increments. Both have a sprite renderer, the End has the EndOfLevelController.cs.
#### PointsManager
just contains all the UI and point management scripts, EnemyKillCountController.cs, HealthTextController.cs, InGameMenuController.cs, ScoreTrackerController.cs, SaveLoadController.cs
#### Enemies
There are 3 types of enemies. Has rigid body, spriteRenderer, collider, animator and scripts. All enemies contain the basic behaviour contained in BasicEnemy.cs and HealthController.cs, then contain the corresponding script to theyr color, by that I mean the other scripts in the Enemy folder. Mainly it adds a type of attack. Also contain the pathfinding from the Astar Pathfinding Project that I used for pathfinding. By scaning the map ones these enemies can find the shortest paths to theyr target.
#### Spawners
Has Sprite renderer, collider, HealthController.cs and SpawnerController.cs the object spawns enemies around itself and can be destroyd by attacking it.
#### Bullet 
Contains a sprite renderer rigidbody collider and a BulletController.cs script that manages it by checking for collisions.
#### Main Dungeon generator and Content Generator
Main Dungeon generator just contains the RoomGenerator.cs script and it calls it on Awake and other object call it for restart for example. DungeonContentGenerator contains just the DungeonContentGenerator script it could have been in the same object.
#### Camera Helper
This object is just designed that the Cinemachine Camera follows it. The CameraController.cs script makes it move in the direction the player is looking. 
#### Helping Hierarchical Objects like Enemies
Just makes sure that the Unity hierarchy is always conprehensible and helps with erasure certain objects
#### MusicManager
Just player on music on a loop
#### Background
just a giant sprite to cover default unity background
### Scripts

#### Game

##### Bullet
- **FlameController.cs**:
Just checks if something entered the flame triggered and then calls the takedamage function.
- **ExplosionController.cs**:
Just makes the objects exist for a bit so it can run the animation and then destroys itself, it is purerly a controller of a visual object.
- **BombController.cs**:
Waits a certain amount of time then checks for all the objects in the collider and then calls a takedamage function.
- **BulletController.cs**:
Checks if anything collided with the bullet so it can make it takeDagame. And configurates the bullet.
###### BulletData
Just contains the scriptable object and the varients to have the same bullet objects that just change properties like damage and range.
##### Camera
- **CameraController.cs**:
Just moves the helping camera object to always be between the mouse and the player.
##### Entity
Has more specific folders.
- **HealthController.cs**:
HealthController.cs that allows objects to call the take damage. 
###### Enemy
Contains the following scripts:
- **BasicEnemy.cs**:  
  Handles configuration and behavior for the basic enemy. This script:
  - Determines enemy behavior in relation to pathfinding.
  - Checks the distance and visibility of the player to decide on actions.

- **IEnemy.cs**:  
  An interface that all enemies inherit from, providing a consistent structure for enemy behavior and interactions.
  - **GreenEnemyController.cs**:  
  Gives the green enemy a specific attack of shooting a projectile, it is identical to PurpleEnemyController.cs just theyr bullets and sprites are different.
  - **PurpleEnemyController.cs**:  
   Gives the purple enemy a specific attack of shooting a projectile, it is identical to GreenEnemyController.cs just theyr bullets and sprites are different.
  - **RedEnemsController.cs**:  
 Overrides the enemyBehaviour function so it can run  away from the bomb it places, at sets up its attack as PlantBomb.

###### Player
  - **PlayerMovementController.cs**:  
  Manages movement of the player relating to the input, used inputsystem package but due to confusion mostly used the default.
    - **PlayerStatsController.cs**:  
  Counts how many enemies of a specific type have been killed and manages the transformations(infections).
    - **PlayerShootingController.cs**:  
  Manages the FlameThrower and bullet shooting, by that I mean theyr spawning, initialization and direction, also checks to turn off the flameThrower.
###### Spawner
  - **SpawnerController.cs**:  
Contains the SpawnerController.cs that spawns enemies around the spawner, it checks if some of them died via event so it can spawn more.
##### Management
Scripts that manage scores and points. ScoreTrackerController.cs manages the incrementation of score when ending the level, EnemyKillCountControler.cs manages points earned after killing an enemy. 
##### Procedural Generation
The procedural generation scripts are seperated into many folders, the Abstracs contain an abstract class the Algorithms are just static. The main generation is done by using Binary Space partitioning algorithm and then either randomly or semi randomly placing tiles spawners and other, or calculated like the start and end of level. I used this video as a tutorial for the Binary Space Partitioning Algorithms: https://www.youtube.com/watch?v=-QOCX6SVFsk&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v
 - **AbstractDungeonGenerator.cs**:  
Makes sure the dungeon generator has some basic fields and functions, this scritpt is pretty obsolete.
 - **BinarySpacePartitioningAlgorithms.cs**:  
The main algorithms that takes parameters and genertes a bunch of BoundsInt that represent the rooms. Has function BinarySpacePartitioning which does it.
 - **DungeonContentGeneratorAlgorithms.cs**:  
Its for two algorithms one PlaceCoordinatesCircularPatternInRoom just generates a bunch of coordinates for spawners in a circular pattern. The other GetTwoRoomsFurthestFromEachOther just find two rooms furthest from each other to decided where the start and end is.
 - **DungeonContentGenerator.cs**:  
Destroys all the enemies and spawners in a reset and manages the placement of spawners and the pathfinding.
 - **RoomConnectAlgorithm.cs**:  
A static class that has an algorithm that with room centers generates paths between them by searching for the closes center to the current on in a expanding maze it ignores the fact that it cuts through other rooms, I believe it makes more interesting level layouts.
 - **RoomGenerator.cs**:  
Contains the RunProceduralGeneration, that ereses the old dungeon and generates a new one. It is the main script for generation that calls all the algorithms and then visualizes them.
 - **TileMapVisualizes.cs**:  
COntains the tilemaps, can clear them get coordinates of a tile converted to a real unity coordinate and also can visualize the whole level if given coordinates of the floor, it will paint even the passable wals for perspecitve.
 - **WallGenerator.cs**:  
Just adds a wall tile next to all the floor tiles and even checks for corners.
##### Structures
Contains scripts for structures, in this case just the EndOFLevel controller that just checks if someonene entered the end of the level.
#### MenusAndUI
Scripts that are for menus and UI. HealthTextController.cs just manages the health UI. InGameMenuController.cs manages the menu that pops up in the game. MainMenuController.cs just manages the firsts scene UI. SaveLoadController.cs manages the saving and loading.