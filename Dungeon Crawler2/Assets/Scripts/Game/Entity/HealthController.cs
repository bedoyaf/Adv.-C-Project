using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour, IDamageable
{
    //configurable
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private float damageModifier = 1f; //so I can adjust how much damage that player/enemy takes
    public int currentHealth { get; private set; }
    //events
    [SerializeField] public UnityEvent<int> onTakeDamage;
    [SerializeField] public UnityEvent<GameObject> onDeathEvent;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// just subtracts from currentHealth with rescpets to damageModifier, and if needed makes the object die dead
    /// </summary>
    public void TakeDamage(int damage)
    {
        damage =(int)(damage * damageModifier);
        currentHealth -= damage;
        onTakeDamage?.Invoke(damage);
        Debug.Log($"{gameObject.name} took {damage} damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Increments the currentHealth, not exceeding maxHealth
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth >maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    /// <summary>
    /// invokes the Die event
    /// </summary>
    public void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        onDeathEvent?.Invoke(gameObject);
    }
    public void LoadHealth(SaveData saveData)
    {
        currentHealth = saveData.Health;
    }
}
