using UnityEngine;

[CreateAssetMenu(fileName = "SimpleBulletBehaviour", menuName = "Scriptable Objects/SimpleBulletBehaviour")]
public class SimpleBulletBehaviour : ProjectileBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated

    public override void Shoot(Weapon weapon)
    {
        // Instantiate the bullet prefab at the weapon's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation);

        // Get the Rigidbody component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Calculate the direction to shoot the bullet based on the weapon's forward direction
        Vector3 shootDirection = weapon.transform.up;

        // Apply force to the bullet in the shoot direction
        rb.linearVelocity = shootDirection * weapon.stats.bulletSpeed;
    }
}