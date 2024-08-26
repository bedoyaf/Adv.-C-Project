using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject bulletPrefab;    
    [SerializeField] private GameObject bombPrefab;       
    private Rigidbody2D rb;
    [SerializeField] BulletData longRangeBullet;
    [SerializeField] BulletData shortRangeBullet;
    private BulletData currentBulletData;
    private ColorEnemy currentInfection  = ColorEnemy.Purple;
    [SerializeField] private GameObject flamePrefab;          
    [SerializeField] private Transform flamePoint;
    private float flameRate = 0.1f;
    private bool isFiring = false;
    private GameObject currentFlameInstance;
    private float flameDistanceFromPlayer = 2f;
    [SerializeField] private float flamerCooldown =1f;
    private float lastFlame = 0f;

    [SerializeField] private float delayBetweenShots = 1f; 
    private float lastShotTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FlipSpriteIfNecessary();

        if (Input.GetMouseButtonDown(1)) // 0 = left mouse button
        {
            Attack();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartFlameThrower();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopFlameThrower();
        }
            UpdateFlamePointDirection();
        

    }

    private void FlipSpriteIfNecessary()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void StopFlameThrower()
    {
        if (isFiring)
        {
            isFiring = false;
            Destroy(currentFlameInstance);  // Destroy the current flame instance when the player releases the button
        }
    }
    private void StartFlameThrower()
    {
        if (Time.time >= lastFlame + flamerCooldown)
        {
            if (!isFiring)
            {
                lastFlame = Time.time;
                isFiring = true;
                currentFlameInstance = Instantiate(flamePrefab, flamePoint.position, flamePoint.rotation, flamePoint);
            }
        }
    }
    public void SetInfection(ColorEnemy newInfection)
    {
        currentInfection = newInfection;
        switch (currentInfection)
        {
            case ColorEnemy.Purple:
                currentBulletData = longRangeBullet;
                break;
            case ColorEnemy.Green:
                currentBulletData = shortRangeBullet;
                break;
        }
    }

    public void Attack()
    {
        if (Time.time - lastShotTime > delayBetweenShots)
        {
            if(currentInfection == ColorEnemy.Purple)
            {
                Shoot();
            }
            else if(currentInfection == ColorEnemy.Red)
            {
                PlantBomb();
            }
            else if(currentInfection == ColorEnemy.Green)
            {
                Shoot();
            }
            else
            {
                Debug.Log("No infection");
            }
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        float bulletSpeed = rb.velocity.magnitude;//*dotProduct;
                                                  // Debug.Log(bulletSpeed);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 currentPosition = transform.position;

        bullet.GetComponent<BulletController>().SetDirection(shootDirection, currentPosition, bulletSpeed);
        bullet.GetComponent<BulletController>().SetTag(gameObject);
        bullet.GetComponent<BulletController>().setBulletData(currentBulletData);

    }

    void PlantBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    private void UpdateFlamePointDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 directionToMouse = (mousePosition - transform.position).normalized;

        flamePoint.position = transform.position + directionToMouse * flameDistanceFromPlayer;

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        flamePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
