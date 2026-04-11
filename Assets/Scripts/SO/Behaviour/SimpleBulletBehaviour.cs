using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SimpleBulletBehaviour", menuName = "Scriptable Objects/SimpleBulletBehaviour")]
public class SimpleBulletBehaviour : WeaponBehaviour
{
    [FormerlySerializedAs("bulletPrefab")]
    public GameObject BulletPrefab; // Prefab of the bullet to be instantiated

    public override void Shoot(Weapon weapon)
    {
        GameObject bullet = Instantiate(BulletPrefab, weapon.ProjectileSpawnPosition, weapon.ProjectileSpawnRotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = weapon.AimDirection;

        rb.linearVelocity = shootDirection * weapon.Stats.ProjectileSpeed;
    }
}