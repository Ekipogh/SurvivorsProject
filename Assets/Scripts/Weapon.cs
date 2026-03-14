using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats stats;
    public List<Enemy> enemyList = new();
    private Enemy _targetEnemy; // The enemy currently targeted for attack

    private bool _hasLoggedMissingStats;
    private bool _hasLoggedMissingProjectileBehaviour;
    private bool _hasLoggedMissingFiringPoint;

    private float attackTimer = 0f; // Timer to track attack cooldown

    public ProjectileBehaviour projectileBehaviour;

    public Transform firingPoint; // The point from which the projectile will be fired

    public Player player; // Reference to the player

    private void Update()
    {
        if (!HasRequiredReferences())
        {
            return;
        }

        // Weapon behaviour:
        // 1. Choose a target according to targeting behaviour. TODO: Implement targeting behaviour as a ScriptableObject
        // 2. Aim at the target
        // 3. If aimed at the target, attack the target
        ChooseTarget();
        var isAimed = Aim();
        if (isAimed)
        {
            AttackEnemy();
        }
    }

    private bool HasRequiredReferences()
    {
        bool hasAllReferences = true;

        if (stats == null)
        {
            if (!_hasLoggedMissingStats)
            {
                Debug.LogWarning($"{name}: Missing WeaponStats reference.", this);
                _hasLoggedMissingStats = true;
            }
            hasAllReferences = false;
        }

        if (projectileBehaviour == null)
        {
            if (!_hasLoggedMissingProjectileBehaviour)
            {
                Debug.LogWarning($"{name}: Missing ProjectileBehaviour reference.", this);
                _hasLoggedMissingProjectileBehaviour = true;
            }
            hasAllReferences = false;
        }

        if (firingPoint == null)
        {
            if (!_hasLoggedMissingFiringPoint)
            {
                Debug.LogWarning($"{name}: Missing firingPoint reference.", this);
                _hasLoggedMissingFiringPoint = true;
            }
            hasAllReferences = false;
        }

        return hasAllReferences;
    }

    private void ChooseTarget()
    {
        // Find the closest enemy within range and the attack angle
        Enemy closestEnemy = null;
        if (enemyList != null && enemyList.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Enemy enemy in enemyList)
            {
                if (enemy == null)
                {
                    continue;
                }

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
        if (_targetEnemy == null)
        {
            // Reset the attack timer if no target is found
            attackTimer = 0f;
            return;
        }

        // Check if the attack cooldown has elapsed
        attackTimer += Time.deltaTime;
        if (attackTimer >= stats.cooldownTime)
        {
            Debug.Log("Attacking enemy: " + _targetEnemy.name);
            projectileBehaviour.Shoot(this);
            attackTimer = 0f; // Reset the attack timer
        }
    }

    private bool Aim()
    {
        // if there is a target enemy, rotate towards it
        // if the target is within range and angle, return true
        if (_targetEnemy == null)
        {
            return false; // No target to aim at
        }

        var isInRange = RotateTowardsTarget();
        return isInRange;
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
