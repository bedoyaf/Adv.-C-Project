using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Generator of the contents of the dungeon that are not tiles, like spawners of enemies
/// </summary>
public class DungeonContentGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject spawner;

    GameObject parentSpawner;

    [SerializeField]
    private GameObject start, end;


    public void DestroySpawners()
    {
        if (parentSpawner != null)
        {
            foreach (Transform child in parentSpawner.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
    /// <summary>
    /// Just places the start and end of the level
    /// </summary>
    public void PlaceStartAndEnd(Vector2Int startPos, Vector2Int endPos, TileMapVisualizer tileMapVisualizer)
    {
        start.transform.position = tileMapVisualizer.GetRealCoordsFromFloorTileMap(startPos);
        end.transform.position =  tileMapVisualizer.GetRealCoordsFromFloorTileMap(endPos);
    }
    /// <summary>
    /// Deletes old spawners and spawns the new ones with already generated coords
    /// </summary>
    public void PlaceSpawners(List<List<Vector2Int>> spawnersForEachRoom, List<ColorEnemy> colors, TileMapVisualizer tileMapVisualizer)
    {
        if (parentSpawner == null)
        {
            parentSpawner = new GameObject("Spawners");
        }
        DestroySpawners();

        for (int i = 0; i < colors.Count; i++)
        {
            foreach (var spawnerPos in spawnersForEachRoom[i])
            {
                GameObject newSpawner = Instantiate(spawner, tileMapVisualizer.GetRealCoordsFromFloorTileMap(spawnerPos), Quaternion.identity, parentSpawner.transform);
                SpriteRenderer spriteRenderer = newSpawner.GetComponent<SpriteRenderer>();
                spriteRenderer.color = GetColorFromEnum(colors[i]);
            }
        }
    }
    private UnityEngine.Color GetColorFromEnum(ColorEnemy colorEnemy)
    {
        switch (colorEnemy)
        {
            case ColorEnemy.Red:
                return UnityEngine.Color.red;
            case ColorEnemy.Green:
                return UnityEngine.Color.green;
            case ColorEnemy.Purple:
                return new UnityEngine.Color(0.5f, 0, 0.5f); // Purple
            default:
                return UnityEngine.Color.white;
        }
    }
}
