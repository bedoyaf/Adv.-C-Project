using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Pursue
}
public interface IEnemy 
{
    public abstract void Attack();
    public abstract void FlipSprite(Transform target);
}
