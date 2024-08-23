using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms 
{
    // Start is called before the first frame update
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var previousposition = startPos;
        for(int i = 0; i < walkLength; i++)
        {
            var newPos=previousposition + RandomDirectionGenerator.GetRandomDirection();
            path.Add(newPos);
            previousposition = newPos;
        }
        return path;
    }
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int length)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = RandomDirectionGenerator.GetRandomDirection();
        var currentPos = startPos;
        corridor.Add(currentPos);
        for(int i = 0; i< length;i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }
        return corridor;
    }
}


