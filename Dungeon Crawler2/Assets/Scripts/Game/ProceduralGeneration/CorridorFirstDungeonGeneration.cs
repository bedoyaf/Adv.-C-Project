using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = 0.8f;
    

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomsPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPos, potentialRoomsPositions);
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomsPositions);
        floorPos.UnionWith(roomPositions);

        tileMapVisualizer.PaintFloorandPerspectiveWallTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tileMapVisualizer);
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomsPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomsNum = Mathf.RoundToInt(potentialRoomsPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomsPositions.OrderBy(x => Guid.NewGuid()).Take(roomsNum).ToList();

        foreach(var roompos in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roompos);

            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomsPositions)
    {
        var currentPosition = startPosition;
        potentialRoomsPositions.Add(currentPosition);

        for (int i = 0; i<corridorCount;i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorCount);
            currentPosition = corridor[corridor.Count-1];
            potentialRoomsPositions.Add(currentPosition);   
            floorPos.UnionWith(corridor);
        }
    }
}
