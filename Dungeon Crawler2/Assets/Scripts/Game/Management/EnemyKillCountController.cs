using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyKillCountController : MonoBehaviour
{
    [SerializeField] GameObject enemyTypeRed;
    [SerializeField] GameObject enemyTypePurple;
    [SerializeField] GameObject enemyTypeGreen;

    [SerializeField] TextMeshProUGUI textRedPoints;
    [SerializeField] TextMeshProUGUI textPurplePoints;
    [SerializeField] TextMeshProUGUI textGreenPoints;

    [SerializeField] private int RedPoints = 0;
    [SerializeField] private int PurplePoints = 0;
    [SerializeField] private int GreenPoints = 0;
    void Start()
    {
        enemyTypeRed.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
        enemyTypeGreen.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
        enemyTypePurple.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        Debug.Log("zavolal se event");
        ColorEnemy enemyType = enemy.GetComponent<BasicEnemy>().colorOfEnemy;
        switch (enemyType)
        {
            case ColorEnemy.Purple:
                PurplePoints++;
                break;
            case ColorEnemy.Green:
                GreenPoints++;
                break;
            case ColorEnemy.Red:
                RedPoints++;
                break;
        }
        UpdateTextPoints();
    }

    private void UpdateTextPoints()
    {
        textRedPoints.text = "R: " + RedPoints;
        textGreenPoints.text = "G: " + GreenPoints;
        textPurplePoints.text = "P: " + PurplePoints;
    }
}
