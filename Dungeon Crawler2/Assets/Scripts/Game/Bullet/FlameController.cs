using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    [SerializeField] private int damage = 10;             

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hor");
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spawner"))
        {
            IDamageable enemy = collision.GetComponent<IDamageable>();  
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
