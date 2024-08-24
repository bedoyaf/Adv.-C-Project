using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField]
    protected Transform spawnLocationsParent;
    protected GameObject spawnLocation;
    protected bool spriteFlipCustomizer = true;
    protected Rigidbody2D _rigidBody;

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
    }
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void setSpawnLocationsParent(Transform spawnLocsparent)
    {
        spawnLocationsParent = spawnLocsparent;
        spawnLocation = new GameObject(gameObject.name + "_SpawnLocation");
        spawnLocation.transform.position = transform.position;
        spawnLocation.transform.parent = spawnLocationsParent;
    }

    void Update()
    {
        
    }


    public abstract void Attack();

    protected void Idle()
    {

    }

    protected void Pursue()
    {

    }

    protected IEnumerator EnemyBehavior()
    {
        while (true)
        {
             float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > EnemyVision)
            {
                destinationSetter.target = spawnLocation.transform;
                isShooting = false; 
            }
            else
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
                    FlipSprite(target);

                    if (distanceToTarget > shootingRange || hasObstacle)
                    {
                       
                        aiPath.canMove = true;
                        destinationSetter.target = target;
                        isShooting = false;
                    }
                    else
                    {
                        // In range and no obstacle, start shooting
                    //    aiPath.canMove = false;
                        isShooting = true;
                    }

                    if (isShooting)
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
                }
            }
         
            yield return null;  
        }
    }

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
