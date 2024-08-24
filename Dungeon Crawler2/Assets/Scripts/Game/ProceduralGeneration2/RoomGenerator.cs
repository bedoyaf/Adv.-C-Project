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
   // [SerializeField]
   // protected TileMapVisualizer tileMapVisualizer = null;
  //  [SerializeField]
 //   protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField]
    private int minRoomHeight = 4, minRoomWidth = 4, DungeonHeight = 20, DungeonWidth = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1; //space between rooms
    [SerializeField]
    private int tilesForOneSpawner = 30, dispersionOfSpawners = 3;

    [SerializeField]
    DungeonContentGenerator dungeonContentGenerator;

    public void Awake()
    {
        RunProceduralGeneration();
    }
    protected override void RunProceduralGeneration()
    {
        tileMapVisualizer.Clear();
        CreateRooms();
    }
    /// <summary>
    /// Runs through all the steps from creation to vizualization
    /// </summary>
    private void CreateRooms()
    {
        var rooms = BinarySpacePartitioningAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(DungeonWidth, DungeonHeight, 0)), minRoomWidth, minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateEachRoom(rooms);
        var centersOfRooms = GetCentersOfRooms(rooms);
        floor.UnionWith(RoomConnectAlgorithm.ConnectRooms(centersOfRooms));
        tileMapVisualizer.PaintFloorandPerspectiveWallTiles(floor);

        List<ColorEnemy> roomColors = new List<ColorEnemy>();
        GenerateRandomColorForRooms(rooms, floor, roomColors);

        var startAndEnd = DungeonContentGeneratorAlgorithms.GetTwoRoomsFurthestFromEachOther(centersOfRooms, floor);



        WallGenerator.CreateWalls(floor, tileMapVisualizer);

        tileMapVisualizer.SetTileMapZToZero();

        dungeonContentGenerator.BakeNavMesh(tileMapVisualizer.GetWallBounds());

        PlaceSpawners(roomColors, rooms);


        dungeonContentGenerator.PlaceStartAndEnd(startAndEnd.Item1, startAndEnd.Item2, tileMapVisualizer);
    }


    private void PlaceSpawners(List<ColorEnemy> colors, List<BoundsInt> rooms)
    {
        List<List<Vector2Int>> spawnersForEachRoom = new List<List<Vector2Int>>();
        foreach (var room in rooms)
        {
            int numOfSpawners = (room.size.x*room.size.y)/tilesForOneSpawner+UnityEngine.Random.Range(-dispersionOfSpawners, dispersionOfSpawners);
            List<Vector2Int> currentRoomSpawnerCoords = DungeonContentGeneratorAlgorithms.PlaceCoordinatesCircularPatternInRoom(room, numOfSpawners);
            spawnersForEachRoom.Add(currentRoomSpawnerCoords);
        }
        dungeonContentGenerator.PlaceSpawners(spawnersForEachRoom, colors, tileMapVisualizer);
    }

    /// <summary>
    /// Just randomly selects a color and the tilemapVizualizer does the rest
    /// </summary>
    private void GenerateRandomColorForRooms(List<BoundsInt> rooms, HashSet<Vector2Int> floor, List<ColorEnemy> roomColors)
    {
        foreach(var room in rooms)
        {
            ColorEnemy color = ColorsOfTheEnemies.GetRandomColor();
            roomColors.Add(color);
            tileMapVisualizer.ColourPaintRoom(room, floor, color);
        }
    }

    /// <summary>
    /// Calculates centers of rooms so we can then connect paths between them
    /// </summary>
    private List<Vector2Int> GetCentersOfRooms(List<BoundsInt> rooms)
    {
        List<Vector2Int> centers = new List<Vector2Int>();
        foreach(var room in rooms)
        {
            centers.Add(new Vector2Int((room.min.x+room.max.x)/2, (room.min.y+room.max.y)/2));
        }
        return centers;
    }

    /// <summary>
    /// When we have the cut rooms from the algorithm it just stores the floor tiles with consideration to the offset, so the rooms are not to close to each other
    /// </summary>
    private HashSet<Vector2Int> CreateEachRoom(List<BoundsInt> rooms) //Todo customize
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach(var room in rooms)
        {
            for (int i = offset; i < room.size.x - offset; i++)
            {
                for(int j = offset; j<room.size.y-offset;j++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(i, j);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
