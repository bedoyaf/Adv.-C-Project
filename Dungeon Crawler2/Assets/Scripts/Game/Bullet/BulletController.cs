using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector3 originalPosition;

    // public Collider2D collider;
    public GameObject owner;
    [SerializeField] float speed = 10f;

     [SerializeField] int damage = 10;
      [SerializeField]
      private float despawnDistance = 20f;
   // [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BulletData bulletData;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setBulletData(BulletData newbulletData)
    {
        bulletData = newbulletData;
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = bulletData.sprite;
        speed = newbulletData.speed;
        damage = newbulletData.damage;
        despawnDistance = newbulletData.despawnDistance;
        transform.Rotate(0,0,bulletData.rotationAngle);
    }

    void Update()
    {
        float distanceTraveled = Vector2.Distance(originalPosition, transform.position);

        if (distanceTraveled > despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetTag(GameObject shooter)
    {
        if (shooter.CompareTag("Player"))
        {
            tag = "FriendlyBullet";
        }
        else if (shooter.CompareTag("Enemy"))
        {
            tag = "EnemyBullet"; 
        }
        else
        {
            Debug.LogWarning("Shooter does not have a valid tag. Cannot set bullet tag.");
        }
    }



    public void SetDirection(Vector2 direction, Vector3 _originalPosition, float additionalbulletSpeed)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        direction = direction.normalized;
        speed += additionalbulletSpeed;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        originalPosition = _originalPosition;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("FriendlyBullet") && collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage); 
            }
            Destroy(gameObject);  
        }
        else if (gameObject.CompareTag("EnemyBullet") && collision.CompareTag("Player"))
        {
            // This is an enemy bullet hitting the player
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);  
            }
            Destroy(gameObject);  
        }
        else if(collision.CompareTag("Structure"))
        {
            Debug.Log("tefil zed");
            Destroy(gameObject);  
        }
    }

}
