using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the damage taken by the flame from the flamethrower the player spawns
/// </summary>
public class FlameController : MonoBehaviour
{
    [SerializeField] private int damage = 10;             

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
