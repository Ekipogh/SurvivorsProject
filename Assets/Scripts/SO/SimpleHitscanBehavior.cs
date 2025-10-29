using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitscanBehavior", menuName = "Scriptable Objects/SimpleHitscanBehavior")]
public class SimpleHitscanBehavior : ProjectileBehaviour
{
    public override void Shoot(Weapon weapon)
    {
        float range = weapon.stats.range;
        float damage = weapon.stats.damageModifier;

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
