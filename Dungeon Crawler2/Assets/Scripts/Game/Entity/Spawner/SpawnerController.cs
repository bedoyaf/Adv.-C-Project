using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private int maxEnemySpawn = 5;
    private int currentNumberOfEnemySpawn = 0;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius=10f;
    [SerializeField] private float spawnInterval = 3f;
    private float spawnTimer = 0f;

    private Transform target;
    private Transform EnemySpawnPointsParent;
    private Transform EnemyParent;
    private UnityAction<GameObject> onDeathCallback;
    void Start()
    {
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(SpawnerDeath);
    }

    public void Initialize(GameObject _enemyPrefab,Transform _target, Transform _enemySpawnPointsParent, Transform _EnemyParent, UnityAction<GameObject> _onDeathCallback)
    {
        enemyPrefab = _enemyPrefab;
        target = _target;
        EnemySpawnPointsParent = _enemySpawnPointsParent;
        EnemyParent = _EnemyParent;
        onDeathCallback = _onDeathCallback;
    }

    public virtual void SpawnerDeath(GameObject dead)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNumberOfEnemySpawn<maxEnemySpawn)
        {
            spawnTimer += Time.deltaTime; 

            if (spawnTimer >= spawnInterval)
            {
                currentNumberOfEnemySpawn++;
                Debug.Log("spawning enemy");
                SpawnEnemy();
                spawnTimer = 0f; 
            }
        }
    }

    private void SpawnEnemy()
    {
        var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        newEnemy.GetComponent<BasicEnemy>().Initialize(target, EnemySpawnPointsParent, EnemyParent, gameObject, onDeathCallback);
    }

    public void OneOfOurSpawnedEnemiesDies()
    {
        currentNumberOfEnemySpawn--;
    }

}
