using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemyController : BasicEnemy
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private BulletData bulletData;
    void Start()
    {
        ConfigurateBasicFields();
        StartCoroutine(EnemyBehavior());
        spriteFlipCustomizer = false;
        colorOfEnemy = ColorEnemy.Purple;
    }
    public override void Attack()
    {
        Shoot();
    }
    private void Shoot()
    {
        Vector2 shootDirection = (target.position - transform.position).normalized;
        float bulletSpeed = _rigidBody.velocity.magnitude;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        GameObject bullet = Instantiate(bulletPrefab, currentPosition, Quaternion.identity);
        bullet.GetComponent<BulletController>().setBulletData(bulletData);
        bullet.GetComponent<BulletController>().SetDirection(shootDirection, currentPosition, bulletSpeed);
        bullet.GetComponent<BulletController>().SetTag(gameObject);
    }
}
