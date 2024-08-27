using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Main Dungeon Generation
/// </summary>
public class ProceduralGenerationRoomGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    private int minRoomHeight = 4, minRoomWidth = 4, DungeonHeight = 20, DungeonWidth = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1; //space between rooms
    [SerializeField]
    private int tilesForOneSpawner = 30, dispersionOfSpawners = 3;

    [SerializeField] private DungeonContentGenerator dungeonContentGenerator; //handles placing spawners and other

    public void Awake()
    {
        RunProceduralGeneration();
    }
    /// <summary>
    /// The main function that Generates the whole level
    /// </summary>
    public override void RunProceduralGeneration()
    {
        tileMapVisualizer.Clear();
        GenerateNewDungon();
    }
    /// <summary>
    /// Runs through all the steps from creation to vizualization
    /// </summary>
    private void GenerateNewDungon()
    {
        //generates rooms
        var rooms = BinarySpacePartitioningAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(DungeonWidth, DungeonHeight, 0)), minRoomWidth, minRoomHeight);
        //extracts floor positions
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateEachRoom(rooms);
        //gets centers of rooms
        var centersOfRooms = GetCentersOfRooms(rooms);
        //connects rooms by corridors
        floor.UnionWith(RoomConnectAlgorithm.ConnectRooms(centersOfRooms));
        //paints the map
        tileMapVisualizer.PaintFloorandPerspectiveWallTiles(floor);
        //finds the start and end of the level, then removes them from room centers
        var startAndEnd = DungeonContentGeneratorAlgorithms.GetTwoRoomsFurthestFromEachOther(centersOfRooms, floor);
        List<BoundsInt> roomsWithoutStartAndEnd = RemoveRoomsByLocation(rooms, centersOfRooms, new List<Vector2Int> { startAndEnd.Item1, startAndEnd.Item2 });
        //generates what rooms spawns what type of enemies and colors it aproprietly
        List<ColorEnemy> roomColors = new List<ColorEnemy>();
        GenerateRandomColorForRooms(roomsWithoutStartAndEnd, floor, roomColors);
        //generates walls of rooms
        WallGenerator.CreateWalls(floor, tileMapVisualizer);
        //scans for pathfinding
        dungeonContentGenerator.ScanAreaForPathFinding();
        //places spawners
        PlaceSpawners(roomColors, roomsWithoutStartAndEnd);
        dungeonContentGenerator.DestroyEnemies();
        //places the start and end of the level
        dungeonContentGenerator.PlaceStartAndEnd(startAndEnd.Item1, startAndEnd.Item2, tileMapVisualizer);
    }

    /// <summary>
    /// acording to the colors generates the spawner coords and then calls to place them in the dungeon 
    /// </summary>
    /// <param name="colors">enums of colors coresponding to each enemy type</param>
    /// <param name="rooms">list of room bounds</param>
    private void PlaceSpawners(List<ColorEnemy> colors, List<BoundsInt> rooms)
    {
        List<List<Vector2Int>> spawnersForEachRoom = new List<List<Vector2Int>>();
        foreach (var room in rooms)
        {
            int numOfSpawners = (room.size.x * room.size.y) / tilesForOneSpawner + UnityEngine.Random.Range(-dispersionOfSpawners, dispersionOfSpawners);
            List<Vector2Int> currentRoomSpawnerCoords = DungeonContentGeneratorAlgorithms.PlaceCoordinatesCircularPatternInRoom(room, numOfSpawners);
            spawnersForEachRoom.Add(currentRoomSpawnerCoords);

        }
        dungeonContentGenerator.PlaceSpawners(spawnersForEachRoom, colors, tileMapVisualizer);
    }
    /// <summary>
    /// Juist removes the rooms containing the points in ignoreTheseRooms from the rooms list
    /// </summary>
    /// <param name="rooms">bounds of all the rooms</param>
    /// <param name="centers">coordinates of all the centers of rooms</param>
    /// <param name="ignoreTheseRooms">Rooms that we want to extract from the list of rooms</param>
    /// <returns>returns the new list of rooms without those we wanted to ignore</returns>
    private List<BoundsInt> RemoveRoomsByLocation(List<BoundsInt> rooms, List<Vector2Int> centers, List<Vector2Int> ignoreTheseRooms)
    {
        List<BoundsInt> newRooms = rooms.ToList() ;
        List<BoundsInt> elementsToDelete = new List<BoundsInt>();
        for (int i=0; i< centers.Count;i++) 
        {
            foreach(var point in ignoreTheseRooms)
            {
                if (centers[i] == point)
                {
                    elementsToDelete.Add(newRooms[i]);
                }
            }
        }

        foreach(var room in elementsToDelete)
        {
            newRooms.Remove(room);
        }
        return newRooms;

    }

    /// <summary>
    /// Just randomly selects a color and the tilemapVizualizer does the rest
    /// </summary>
    /// <param name="rooms">bounds of all the rooms</param>
    /// <param name="floor">all the coordinates of the floor</param>
    /// <param name="roomColors">All the generates colors of enemytypes for each room</param>
    private void GenerateRandomColorForRooms(List<BoundsInt> rooms, HashSet<Vector2Int> floor, List<ColorEnemy> roomColors)
    {
        foreach (var room in rooms)
        {
            ColorEnemy color = ColorsOfTheEnemies.GetRandomColor();
            roomColors.Add(color);
            tileMapVisualizer.ColourPaintRoom(room, floor, color);
        }
    }

    /// <summary>
    /// Calculates centers of rooms so we can then connect paths between them
    /// </summary>
    /// <param name="rooms">bounds of all the rooms</param>
    /// <returns>list of all the coordinates of the centers of rooms</returns>
    private List<Vector2Int> GetCentersOfRooms(List<BoundsInt> rooms)
    {
        List<Vector2Int> centers = new List<Vector2Int>();
        foreach (var room in rooms)
        {
            centers.Add(new Vector2Int((room.min.x + room.max.x) / 2, (room.min.y + room.max.y) / 2));
        }
        return centers;
    }

    /// <summary>
    /// When we have the cut rooms from the algorithm it just stores the floor tiles with consideration to the offset, so the rooms are not to close to each other
    /// </summary>
    /// <param name="rooms">bounds of all the rooms</param>
    /// <returns>all the coordinates of the floor</returns>
    private HashSet<Vector2Int> CreateEachRoom(List<BoundsInt> rooms) //Todo customize
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in rooms)
        {
            for (int i = offset; i < room.size.x - offset; i++)
            {
                for (int j = offset; j < room.size.y - offset; j++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(i, j);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
