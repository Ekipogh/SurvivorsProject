using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SimpleRocketBehavior", menuName = "Scriptable Objects/SimpleRocketBehavior")]
public class SimpleRocketBehavior : WeaponBehaviour
{
    [FormerlySerializedAs("rocketPrefab")]
    public GameObject RocketPrefab; // Prefab of the rocket to be instantiated

    public override void Shoot(Weapon weapon)
    {
        GameObject rocket = Instantiate(RocketPrefab, weapon.ProjectileSpawnPosition, weapon.ProjectileSpawnRotation);

        if (weapon.TargetEnemy != null)
        {
            SimpleHommingRocket rocketScript = rocket.GetComponent<SimpleHommingRocket>();
            if (rocketScript != null)
            {
                rocketScript.Target = weapon.TargetEnemy.transform;
            }
        }

        SimpleHommingRocket rocketStats = rocket.GetComponent<SimpleHommingRocket>();
        if (rocketStats != null)
        {
            rocketStats.Speed = weapon.Stats.ProjectileSpeed;
            rocketStats.Damage = weapon.Stats.DamageModifier;
            rocketStats.RotationSpeed = weapon.Stats.RotationSpeed;
            rocketStats.ExplosionRadius = weapon.Stats.AoeRadius;
        }
    }
}
