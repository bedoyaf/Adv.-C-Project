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
}

public static class RandomDirectionGenerator
{
    public static List<Vector2Int> directions = new List<Vector2Int>
    {//new Vector2Int(1,1),
        new Vector2Int(0,1),
        new Vector2Int(1,0),
    //    new Vector2Int(0,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
     //   new Vector2Int(-1,-1)
    };
    public static Vector2Int GetRandomDirection()
    {
        return directions[Random.Range(0,directions.Count)];
    }

}
