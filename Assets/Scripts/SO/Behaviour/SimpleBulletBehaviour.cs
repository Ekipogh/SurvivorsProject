using UnityEngine;

[CreateAssetMenu(fileName = "SimpleBulletBehaviour", menuName = "Scriptable Objects/SimpleBulletBehaviour")]
public class SimpleBulletBehaviour : WeaponBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated

    public override void Shoot(Weapon weapon)
    {
        GameObject bullet = Instantiate(bulletPrefab, weapon.ProjectileSpawnPosition, weapon.ProjectileSpawnRotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = weapon.AimDirection;

        rb.linearVelocity = shootDirection * weapon.stats.projectileSpeed;
    }
}