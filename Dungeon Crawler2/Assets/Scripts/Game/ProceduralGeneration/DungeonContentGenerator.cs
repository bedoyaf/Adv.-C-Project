using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.CompositeCollider2D;
using Pathfinding;
using UnityEngine.Tilemaps;
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
    [SerializeField]
    private GameObject Player;
    [SerializeField] private GameObject redEnemy, purpleEnemy, greenEnemy;

    [SerializeField]
    private AstarPath pathfinding;
    [SerializeField]
    private Transform EnemySpawnPointsParent, EnemyParent;
    [SerializeField] private GameObject statsCounter;

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

    public void BakeNavMesh(Bounds bounds)
    {
        StartCoroutine(ScanAfterDelay(0.01f));
    }

    private IEnumerator ScanAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        pathfinding.Scan(); // Perform the scan
    }

    /// <summary>
    /// All the setup for spawning an Enemy
    /// </summary>
    public void spawnEnemy(Vector3 spawnPoint, ColorEnemy color, Transform target)
    {
     /*   GameObject enemyprefab;
        switch (color)
        {
            case ColorEnemy.Purple:
                enemyprefab = purpleEnemyTemp;
                break;
            default:
                enemyprefab = purpleEnemyTemp;
                break;
        }*/
       // var newEnemy = Instantiate(enemyprefab, spawnPoint, Quaternion.identity);
    //    newEnemy.GetComponent<BasicEnemy>().Initialize(target, EnemySpawnPointsParent, EnemyParent, statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath);
       /* newEnemy.GetComponent<BasicEnemy>().setTarget(target);
        newEnemy.GetComponent<BasicEnemy>().setSpawnLocationsParent(EnemySpawnPointsParent);
        newEnemy.GetComponent<BasicEnemy>().setEnemyParent(EnemyParent);*/
      //  newEnemy.GetComponent<HealthController>().onDeathEvent.AddListener(statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath);
    }

    /// <summary>
    /// Just places the start and end of the level
    /// </summary>
    public void PlaceStartAndEnd(Vector2Int startPos, Vector2Int endPos, TileMapVisualizer tileMapVisualizer)
    {
        start.transform.position = tileMapVisualizer.GetRealCoordsFromFloorTileMap(startPos);
        end.transform.position = tileMapVisualizer.GetRealCoordsFromFloorTileMap(endPos);
        Player.transform.position = start.transform.position;
        spawnEnemy(tileMapVisualizer.GetRealCoordsFromFloorTileMap(startPos), ColorEnemy.Purple, Player.transform);
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
            GameObject enemyprefab;
            switch (colors[i])
            {
                case ColorEnemy.Purple:
                    enemyprefab = purpleEnemy;
                    break;
                case ColorEnemy.Red:
                    enemyprefab = redEnemy;
                    break;
                case ColorEnemy.Green:
                    enemyprefab = greenEnemy;
                    break;
                default:
                    enemyprefab = purpleEnemy;
                    break;
            }
            foreach (var spawnerPos in spawnersForEachRoom[i])
            {
                    GameObject newSpawner = Instantiate(spawner, tileMapVisualizer.GetRealCoordsFromFloorTileMap(spawnerPos), Quaternion.identity, parentSpawner.transform);
                    newSpawner.GetComponent<SpawnerController>().Initialize(enemyprefab,Player.transform,EnemySpawnPointsParent, EnemyParent, statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath);
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
