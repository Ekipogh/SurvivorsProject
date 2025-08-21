using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitscanBehavior", menuName = "Scriptable Objects/SimpleHitscanBehavior")]
public class SimpleHitscanBehavior : ProjectileBehaviour
{
    public float range = 100f;
    public float damage = 50f;

    public override void Shoot(Weapon weapon)
    {
        if (weapon.TargetEnemy == null) return;

        // Perform a raycast to detect the target
        RaycastHit2D hit = Physics2D.Raycast(weapon.firingPoint.position, weapon.firingPoint.up, range);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            weapon.TargetEnemy.TakeDamage(damage);
        }
    }
}
