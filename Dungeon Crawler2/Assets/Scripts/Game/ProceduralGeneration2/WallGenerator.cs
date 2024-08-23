using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class WallGenerator : MonoBehaviour
{
    /// <summary>
    /// Just gets the wall positions and them vizualizes them on a different tilemap then the floor
    /// </summary>
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, RandomDirectionGenerator.directions);
        basicWallPositions.UnionWith(patchCorners(floorPositions, RandomDirectionGenerator.directions, basicWallPositions));
        foreach (var position in basicWallPositions)
        {
            tileMapVisualizer.paintSingleBasickWall(position);
        }
    }
    /// <summary>
    /// Goes throgh the floor tiles and if they are adjecant to an empty space, they add that as a wall
    /// </summary>
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directions)
            {
                var neighbourPosition = position + direction;
                if (!floorPositions.Contains(neighbourPosition))
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
    private static HashSet<Vector2Int> patchCorners(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions, HashSet<Vector2Int> wallPositions)
    {
        HashSet<Vector2Int> corners = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            int emptyNeighbours = 0;
            foreach (var direction in directions)
            {
                foreach (var dir in RandomDirectionGenerator.diagonalDirections)
                {
                    var diagonalNeighbour = position + dir;
                    if (!floorPositions.Contains(diagonalNeighbour) && !wallPositions.Contains(diagonalNeighbour))
                    {
                        corners.Add(diagonalNeighbour);
                    }
                }
            }
        }
        return corners;
    }

}
public static class RandomDirectionGenerator
{
    public static List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };
    public static List<Vector2Int> diagonalDirections = new List<Vector2Int>
        {
            new Vector2Int(-1,-1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1),
            new Vector2Int(1,1)
        };
    public static Vector2Int GetRandomDirection()
    {
        return directions[UnityEngine.Random.Range(0, directions.Count)];
    }

}
