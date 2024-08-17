using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 100f;
    public DamageEvent OnDamageTaken;  // Event that will be triggered by the bullet

    void Start()
    {
        if (OnDamageTaken == null)
            OnDamageTaken = new DamageEvent();

        // Optionally, you can add a listener directly in code
        OnDamageTaken.AddListener(TakeDamage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            // Handle the object's destruction or death
            Destroy(gameObject);
        }
    }
}