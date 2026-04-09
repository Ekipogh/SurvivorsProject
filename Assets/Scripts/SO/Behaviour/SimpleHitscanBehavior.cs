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
        Enemy hitEnemy = hit.collider != null && hit.collider.CompareTag("Enemy") ? hit.collider.GetComponent<Enemy>() : null;
        if (hitEnemy != null)
        {
            hitEnemy.TakeDamage(damage);
        }
    }
}
