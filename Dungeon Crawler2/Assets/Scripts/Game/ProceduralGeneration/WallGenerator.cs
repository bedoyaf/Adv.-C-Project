using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class WallGenerator : MonoBehaviour
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, RandomDirectionGenerator.directions);
        foreach(var position in basicWallPositions) {
            tileMapVisualizer.paintSingleBasickWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach(var position in floorPositions)
        {
            foreach(var direction in directions)
            {
                var neighbourPosition = position +direction;
                if(floorPositions.Contains(neighbourPosition) == false)
                {
                    Debug.Log(floorPositions.Count);
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
