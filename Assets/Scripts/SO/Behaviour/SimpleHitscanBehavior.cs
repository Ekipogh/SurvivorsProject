using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitscanBehavior", menuName = "Scriptable Objects/SimpleHitscanBehavior")]
public class SimpleHitscanBehavior : WeaponBehaviour
{
    public override void Shoot(Weapon weapon)
    {
        float range = weapon.stats.range;
        float damage = weapon.stats.damageModifier;

        if (weapon.TargetEnemy == null) return;

        RaycastHit2D hit = Physics2D.Raycast(weapon.ProjectileSpawnPosition, weapon.AimDirection, range);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            weapon.TargetEnemy.TakeDamage(damage);
        }
    }
}
