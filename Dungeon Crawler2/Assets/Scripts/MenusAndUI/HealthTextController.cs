using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class juist keeps the Health data on the UI up to date
/// </summary>
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
        if (healthController != null)
        {
            healthController.onTakeDamage.RemoveListener(UpdateHealthText);
        }
    }
}
