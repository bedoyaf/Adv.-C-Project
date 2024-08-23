using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
    // [SerializeField]
    //  protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;

    [SerializeField]
    private TileMapVisualizer tilemapVizualizer;



    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
      //  Debug.Log("count "+floorPositions.Count); ;
        tilemapVizualizer.Clear();
        tilemapVizualizer.PaintFloorandPerspectiveWallTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
         /*   foreach(var p in path)
            {
                Debug.Log(p);
            }*/
            floorPosition.UnionWith(path);
            if(parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }


}
