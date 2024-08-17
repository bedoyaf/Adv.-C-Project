using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    private Rigidbody2D rb;
    Vector3 originalPosition;
    public float despawnDistance = 10f;
    // public Collider2D collider;
    public GameObject owner;
    [SerializeField] float speed = 10f;

    [SerializeField] float damage = 10f;  // Amount of damage this bullet deals

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // Try to get a Damageable component on the object the bullet hits
        var damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // Invoke the damage event on the target object, passing the damage value
            damageable.OnDamageTaken.Invoke(damage);

            // Destroy the bullet after hitting something
            Destroy(gameObject);
        }
        else/* if (collision.CompareTag("Wall"))*/
        {
            // Destroy the bullet if it hits a wall or other non-damageable object
            Destroy(gameObject);
        }
    }

}
