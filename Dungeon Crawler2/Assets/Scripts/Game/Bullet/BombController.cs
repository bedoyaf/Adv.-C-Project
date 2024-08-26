using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
//    public float ExplosionFadeTime = 2;
//    public Sprite bomb;
//    [SerializeField] private Sprite Explosion;
    [SerializeField] private float explosionTime = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage=30;
 //   public LayerMask explosionLayers;
 //   public AudioSource Source;
   // public AudioClip BoomClip;
    public GameObject explosionEffect;


    private bool exploded = false;
    private float elapsedTime = 0f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the countdown to explosion
        Invoke("Explode", explosionTime);
    }

    void Explode()
    {
        if (exploded) return;

        exploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    void Update()
    {
        Invoke("Explode", explosionTime);
    }
}
