using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRocketBehavior", menuName = "Scriptable Objects/SimpleRocketBehavior")]
public class SimpleRocketBehavior : WeaponBehaviour
{
    public GameObject rocketPrefab; // Prefab of the rocket to be instantiated

    public override void Shoot(Weapon weapon)
    {
        // Instantiate the rocket prefab at the weapon's position and rotation
        GameObject rocket = Instantiate(rocketPrefab, weapon.transform.position, weapon.transform.rotation);

        // Set rocket's target to the current target of the weapon
        if (weapon.TargetEnemy != null)
        {
            SimpleHommingRocket rocketScript = rocket.GetComponent<SimpleHommingRocket>();
            if (rocketScript != null)
            {
                rocketScript.target = weapon.TargetEnemy.transform; // Set the target for the rocket to follow
            }
        }

        // Set rocket stats based on the weapon's stats
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
