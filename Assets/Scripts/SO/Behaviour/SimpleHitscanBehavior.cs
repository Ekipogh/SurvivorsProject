using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitscanBehavior", menuName = "Scriptable Objects/SimpleHitscanBehavior")]
public class SimpleHitscanBehavior : WeaponBehaviour
{
    public override void Shoot(Weapon weapon)
    {
        float range = weapon.Stats.Range;
        float damage = weapon.Stats.DamageModifier;

        if (weapon.TargetEnemy == null) return;

        ContactFilter2D filter = new();
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));
        List<RaycastHit2D> results = new();
        Physics2D.Raycast(weapon.ProjectileSpawnPosition, weapon.AimDirection, filter, results, range);
        RaycastHit2D hit = results.Count > 0 ? results[0] : default;
        Enemy hitEnemy = hit.collider != null && hit.collider.CompareTag("Enemy") ? hit.collider.GetComponent<Enemy>() : null;
        if (hitEnemy != null)
        {
            hitEnemy.TakeDamage(damage);
        }
    }
}
