using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    private const float MinDirectionSqrMagnitude = 0.0001f;

    public WeaponStats Stats;
    public List<Enemy> EnemyList = new();
    private Enemy _targetEnemy; // The enemy currently targeted for attack

    private bool _hasLoggedMissingStats;
    private bool _hasLoggedMissingProjectileBehaviour;
    private bool _hasLoggedMissingFiringPoint;

    private float _attackTimer = 0f; // Timer to track attack cooldown
    public WeaponBehaviour ProjectileBehaviour;
    public Transform FiringPoint; // The point from which the projectile will be fired
    public Player Player; // Reference to the player

    public Vector3 ProjectileSpawnPosition => FiringPoint != null ? FiringPoint.position : transform.position;

    public Quaternion ProjectileSpawnRotation => FiringPoint != null ? FiringPoint.rotation : transform.rotation;

    public Vector2 AimDirection => FiringPoint != null ? FiringPoint.up : transform.up;

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

        if (Stats == null)
        {
            if (!_hasLoggedMissingStats)
            {
                Debug.LogWarning($"{name}: Missing WeaponStats reference.", this);
                _hasLoggedMissingStats = true;
            }
            hasAllReferences = false;
        }

        if (ProjectileBehaviour == null)
        {
            if (!_hasLoggedMissingProjectileBehaviour)
            {
                Debug.LogWarning($"{name}: Missing ProjectileBehaviour reference.", this);
                _hasLoggedMissingProjectileBehaviour = true;
            }
            hasAllReferences = false;
        }

        if (FiringPoint == null)
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
        float halfAttackAngle = Stats.AttackAngle * 0.5f;
        if (IsTargetWithinAngleAndRange(_targetEnemy, halfAttackAngle))
        {
            return;
        }

        Enemy closestEnemy = null;
        if (EnemyList != null && EnemyList.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Enemy enemy in EnemyList)
            {
                if (!IsTargetWithinAngleAndRange(enemy, halfAttackAngle))
                {
                    continue;
                }

                float distance = Vector2.Distance(GetAimOrigin(), enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
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
            _attackTimer = 0f;
            return;
        }

        // Check if the attack cooldown has elapsed
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= Stats.CooldownTime)
        {
            ProjectileBehaviour.Shoot(this);
            _attackTimer = 0f; // Reset the attack timer
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
            Vector2 aimOrigin = GetAimOrigin();
            Vector2 targetDirection = (Vector2)_targetEnemy.transform.position - aimOrigin;
            if (targetDirection.sqrMagnitude <= MinDirectionSqrMagnitude)
            {
                return false;
            }

            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Stats.RotationSpeed * Time.deltaTime);

            float distanceToTarget = targetDirection.magnitude;
            float angleToTarget = Vector2.Angle(AimDirection, targetDirection);
            return distanceToTarget <= Stats.Range && angleToTarget <= Stats.AimTolerance;
        }
        return false; // No target to rotate towards
    }

    public Enemy TargetEnemy
    {
        get { return _targetEnemy; }
    }

    private Vector2 GetAimOrigin()
    {
        return FiringPoint != null ? FiringPoint.position : transform.position;
    }

    private bool IsTargetWithinAngleAndRange(Enemy enemy, float halfAttackAngle)
    {
        if (enemy == null)
        {
            return false;
        }

        Vector2 directionToEnemy = (Vector2)enemy.transform.position - GetAimOrigin();
        if (directionToEnemy.sqrMagnitude <= MinDirectionSqrMagnitude)
        {
            return false;
        }

        float distance = directionToEnemy.magnitude;
        if (distance > Stats.Range)
        {
            return false;
        }

        float angle = Vector2.Angle(AimDirection, directionToEnemy);
        return angle <= halfAttackAngle;
    }
}
