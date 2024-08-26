using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject bulletPrefab;    
    [SerializeField] private GameObject bombPrefab;
    private Transform firePoint;        
    private Rigidbody2D rb;
    [SerializeField] BulletData longRangeBullet;
    [SerializeField] BulletData shortRangeBullet;
    private BulletData currentBulletData;
    private ColorEnemy currentInfection  = ColorEnemy.Purple;

   // private PlayerStatsController playerStatsController;


    [SerializeField] private float delayBetweenShots = 0.5f; 
    private float lastShotTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //flip sprites according to mouse
        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            Attack();
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

}
