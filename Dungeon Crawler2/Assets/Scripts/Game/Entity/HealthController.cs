using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField]
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    [SerializeField] public UnityEvent<int> onTakeDamage;
    [SerializeField] public UnityEvent<GameObject> onDeathEvent;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onTakeDamage?.Invoke(damage);
        Debug.Log($"{gameObject.name} took {damage} damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} died!");
       // Destroy(gameObject);
        onDeathEvent?.Invoke(gameObject);
    }
}
