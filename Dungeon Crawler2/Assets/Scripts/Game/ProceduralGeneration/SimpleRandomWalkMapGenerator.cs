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
    private SimpleRandomWalkSO randomWalkParameters;

    [SerializeField]
    private TileMapVisualizer tilemapVizualizer;

    public void Awake()
    {
        RunProceduralGeneration();
    }

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
      //  Debug.Log("count "+floorPositions.Count); ;
        tilemapVizualizer.Clear();
        tilemapVizualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
         /*   foreach(var p in path)
            {
                Debug.Log(p);
            }*/
            floorPosition.UnionWith(path);
            if(randomWalkParameters.startRandomlyEachIteration)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }


}
