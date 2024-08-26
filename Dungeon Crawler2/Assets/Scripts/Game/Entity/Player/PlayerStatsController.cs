using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    private PlayerShootingController shootingController;
    [SerializeField] private List<RuntimeAnimatorController> animationControllers; //default,green,red, violet
    private Animator animator;
    public ColorEnemy currentInfection = ColorEnemy.Purple;
    void Start()
    {
        var playerHealthController = GetComponent<HealthController>();
        playerHealthController.onDeathEvent.AddListener(PlayerDeath);
        shootingController = GetComponent<PlayerShootingController>();
        animator=GetComponent<Animator>();
        setPlayerInfection();
    }

    private void setPlayerInfection()
    {
        shootingController.SetInfection(currentInfection);
        int index = 0;
        switch (currentInfection)
        {
            case ColorEnemy.Purple:
                index = 3;
                break;
            case ColorEnemy.Green:
                index = 1;
                break;
            case ColorEnemy.Red:
                index = 2;
                break;
            case ColorEnemy.None:
                index = 0;
                break;
        }
        animator.runtimeAnimatorController = animationControllers[index];
    }

    public virtual void PlayerDeath(GameObject dead)
    {
        gameObject.SetActive(false);
    }
}
