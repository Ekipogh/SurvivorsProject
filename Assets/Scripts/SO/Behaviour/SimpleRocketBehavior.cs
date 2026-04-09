using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRocketBehavior", menuName = "Scriptable Objects/SimpleRocketBehavior")]
public class SimpleRocketBehavior : WeaponBehaviour
{
    public GameObject rocketPrefab; // Prefab of the rocket to be instantiated

    public override void Shoot(Weapon weapon)
    {
        GameObject rocket = Instantiate(rocketPrefab, weapon.ProjectileSpawnPosition, weapon.ProjectileSpawnRotation);

        if (weapon.TargetEnemy != null)
        {
            SimpleHommingRocket rocketScript = rocket.GetComponent<SimpleHommingRocket>();
            if (rocketScript != null)
            {
                rocketScript.target = weapon.TargetEnemy.transform;
            }
        }

        SimpleHommingRocket rocketStats = rocket.GetComponent<SimpleHommingRocket>();
        if (rocketStats != null)
        {
            rocketStats.speed = weapon.stats.projectileSpeed;
            rocketStats.damage = weapon.stats.damageModifier;
            rocketStats.rotationSpeed = weapon.stats.rotationSpeed;
            rocketStats.explosionRadius = weapon.stats.aoeRadius;
        }
    }
}
