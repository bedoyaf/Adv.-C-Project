using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public GameObject bulletPrefab;    // The bullet prefab to instantiate
    public Transform firePoint;        // The point from where the bullet will be fired
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;

    public float delayBetweenShots = 0.5f; // Adjust this value to set the delay between shots
    private float lastShotTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
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

        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {

            if (Time.time - lastShotTime > delayBetweenShots)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }

    }

    void Shoot()
    {

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 shootDirection = (mousePosition - transform.position).normalized;

            // Get the player's Rigidbody2D component (assuming the player has one)

            // Calculate the bullet speed by combining the bullet's speed and the player's speed
            // float dotProduct = Vector2.Dot(shootDirection, rb.velocity);
            float bulletSpeed = rb.velocity.magnitude;//*dotProduct;
                                                      // Debug.Log(bulletSpeed);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 currentPosition = transform.position;

            // Set the bullet's direction and speed
            bullet.GetComponent<BulletController>().SetDirection(shootDirection, currentPosition, bulletSpeed);
        
    }

}
