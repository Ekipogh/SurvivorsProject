using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats stats;
    public List<Enemy> enemyList = new();
    private Enemy _targetEnemy; // The enemy currently targeted for attack

    private float attackTimer = 0f; // Timer to track attack cooldown

    public ProjectileBehaviour projectileBehaviour;

    public Transform firingPoint; // The point from which the projectile will be fired

    public Player player; // Reference to the player

    private void Update()
    {
        // Weapon behaviour:
        // 1. Choose a target according to targeting behaviour. TODO: Implement targeting behaviour as a ScriptableObject
        // 2. Aim at the target
        // 3. If aimed at the target, attack the target
        ChooseTarget();
        var isAimed = Aim();
        Debug.Log("Weapon " + gameObject.name + " aiming at target: " + (_targetEnemy != null ? _targetEnemy.name : "None"));
        if (isAimed)
        {
            AttackEnemy();
        }
    }

    private void ChooseTarget()
    {
        // Find the closest enemy within range and the attack angle
        Enemy closestEnemy = null;
        if (enemyList.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Enemy enemy in enemyList)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance && distance <= stats.range)
                {
                    Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                    float angle = Vector2.Angle(transform.up, directionToEnemy);
                    if (angle <= stats.attackAngle / 2f) // Check if within attack angle
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        _targetEnemy = closestEnemy;
    }

    private void AttackEnemy()
    {
        if (_targetEnemy != null)
        {
            // Check if the attack cooldown has elapsed
            attackTimer += Time.deltaTime;
            if (attackTimer >= stats.cooldownTime)
            {
                Debug.Log("Attacking enemy: " + _targetEnemy.name);
                projectileBehaviour.Shoot(this);
                attackTimer = 0f; // Reset the attack timer
            }
        }
        else
        {
            // Reset the attack timer if no target is found
            attackTimer = 0f;
        }
    }

    private bool Aim()
    {
        // if there is a target enemy, rotate towards it
        // if the target is within range and angle, return true
        if (_targetEnemy != null)
        {
            var isInRange = RotateTowardsTarget();
            return isInRange;
        }
        return false; // No target to aim at
    }

    private bool RotateTowardsTarget()
    {
        if (_targetEnemy != null)
        {
            // rotate towards the target enemy
            Vector3 targetDirection = _targetEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, stats.rotationSpeed * Time.deltaTime);

            // Check if the target is within range and angle
            float distanceToTarget = Vector2.Distance(transform.position, _targetEnemy.transform.position);
            float angleToTarget = Vector2.Angle(transform.up, targetDirection);
            return distanceToTarget <= stats.range && angleToTarget <= stats.attackAngle / 2f;
        }
        return false; // No target to rotate towards
    }

    public Enemy TargetEnemy
    {
        get { return _targetEnemy; }
    }
}
