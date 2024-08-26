using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] HealthController healthController;

    public void UpdateHealthText(int damage)
    {

            textMesh.text = $"Health: {healthController.currentHealth}";

    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (healthController != null)
        {
            healthController.onTakeDamage.RemoveListener(UpdateHealthText);
        }
    }
}
