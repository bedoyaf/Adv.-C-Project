using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// class that sets up functions that are universal for every enemy type, mainly pathfinding
/// </summary>
public abstract class BasicEnemy : MonoBehaviour, IEnemy
{
    protected EnemyState currentState = EnemyState.Idle;
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float EnemyVision = 20f;
    [SerializeField]
    protected int movementSpeed = 5;
    [SerializeField]
    protected float shootingRange, shootingCooldown;
    protected float shootTimer;
    //   protected NavMeshAgent agent;
    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;
    //protected 
    private GameObject spawner;
    [SerializeField]
    protected Transform spawnLocationsParent, enemyParent;

    protected GameObject spawnLocation;
    protected bool spriteFlipCustomizer = true;
    protected Rigidbody2D _rigidBody;

    public ColorEnemy colorOfEnemy { get; protected set; }


    bool isShooting = false;

    void Start()
    {
        ConfigurateBasicFields();
        StartCoroutine(EnemyBehavior());
    }

    protected void ConfigurateBasicFields()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        _rigidBody = GetComponent<Rigidbody2D>();
        destinationSetter.target = target;
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(EnemyDeath);

    }

    public void Initialize(Transform newTarget, Transform spawnLocsParent, Transform newEnemyParent, GameObject _mySpawner, UnityAction<GameObject> onDeathCallback)
    {
        setTarget(newTarget);
        setSpawnLocationsParent(spawnLocsParent);
        setEnemyParent(newEnemyParent);
        GetComponent<HealthController>().onDeathEvent.AddListener(onDeathCallback);
        spawner =_mySpawner;
    }

    private void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void setSpawnLocationsParent(Transform spawnLocsparent)
    {
        spawnLocationsParent = spawnLocsparent;
        spawnLocation = new GameObject(gameObject.name + "_SpawnLocation");
        spawnLocation.transform.position = transform.position;
        spawnLocation.transform.parent = spawnLocationsParent;
    }
    private void setEnemyParent(Transform newEnemyParent)
    {
        enemyParent = newEnemyParent;
        gameObject.transform.parent = enemyParent;
    }


    public virtual void EnemyDeath(GameObject dead)
    {
        spawner.GetComponent<SpawnerController>().OneOfOurSpawnedEnemiesDies();
        Destroy(spawnLocation);

        Destroy(gameObject);
    }

    public abstract void Attack();

    protected void Idle()
    {

    }

    /// <summary>
    /// Checks if the player is close enaugh to pursue if yes, then it checks if it has a line of sight, if yes then pursues
    /// </summary>
    protected virtual IEnumerator EnemyBehavior()
    {
        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > EnemyVision)
            {
                destinationSetter.target = spawnLocation.transform;
                isShooting = false;
            }
            else //Player is too far away
            {
                bool hasObstacle = HasObstaclesInFrontOfEnemy();

                if (!hasObstacle)
                {
                    currentState = EnemyState.Pursue;
                }

                if (currentState == EnemyState.Idle)
                {
                    aiPath.canMove = false;
                }
                else
                {
                    Pursue(distanceToTarget, hasObstacle);
                }
            }

            yield return null;
        }
    }
    protected void Pursue(float distanceToTarget, bool hasObstacle)
    {
        FlipSprite(target);

        if (distanceToTarget > shootingRange || hasObstacle)
        {
            aiPath.canMove = true;
            destinationSetter.target = target;
            isShooting = false;
        }
        else
        {
            isShooting = true;
        }

        if (isShooting)
        {
            withConsiderationToTimeShoot();
        }
    }

    protected void withConsiderationToTimeShoot()
    {
        if (shootTimer <= 0f)
        {
            Attack();
            shootTimer = shootingCooldown;  // Reset the cooldown timer
        }
        else
        {
            shootTimer -= Time.deltaTime;  // Decrease the cooldown timer
        }
    }

    /// <summary>
    /// Just flips the sprite correspondingly to the target, so it is always facing the player
    /// </summary>
    public void FlipSprite(Transform lookingPoint)
    {
        float horizontal = -transform.position.x + lookingPoint.position.x;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (horizontal > 0)
        {
            _spriteRenderer.flipX = spriteFlipCustomizer;
        }
        else if (horizontal < 0)
        {
            _spriteRenderer.flipX = !spriteFlipCustomizer;
        }
    }

    /// <summary>
    /// Using Raycast checks for line of sight
    /// </summary>
    public bool HasObstaclesInFrontOfEnemy()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        direction.z = 0;
        LayerMask layerMask = LayerMask.GetMask("Player", "Walls");
        RaycastHit2D hit2D = Physics2D.Raycast(currentPosition, direction, 100, layerMask);

        if (hit2D.collider != null && !hit2D.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }



}
