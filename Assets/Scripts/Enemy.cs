using System;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : GameCharacter
{
    [FormerlySerializedAs("player")]
    public Player Player;

    [FormerlySerializedAs("enemyStats")]
    public EnemyStats EnemyStatsData;

    // States
    private bool _isDamagingPlayer = false;
    private bool _isDead = false;

    private float _damageCooldown = 1f; // damage cooldown time in seconds
    private float _damageTimer = 2f; // timer to track damage cooldown, start able to damage player

    [FormerlySerializedAs("rotatation")]
    public Vector3 Rotation;

    [FormerlySerializedAs("sprite")]
    public Transform Sprite;

    void Update()
    {
        // Move towards the player
        if (Player != null && !_isDamagingPlayer)
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            transform.position += Stats.Speed * Time.deltaTime * (Vector3)direction;
            // rotate towards the player
            Vector3 targetDirection = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Sprite.transform.rotation = Quaternion.RotateTowards(Sprite.transform.rotation, rotation, Stats.RotationSpeed * Time.deltaTime);
        }
        DamagePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isDamagingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isDamagingPlayer = false;
        }
    }

    protected override void Die()
    {
        _isDead = true;
        Player.RewardPoints(EnemyStatsData.PointsValue); // Reward points to the player for defeating this enemy
    }

    private void DamagePlayer()
    {
        if (_isDamagingPlayer && Player != null)
        {
            // Check if the damage cooldown has elapsed
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= _damageCooldown)
            {
                // Deal damage to the player
                Player.TakeDamage(EnemyStatsData.Damage);
                _damageTimer = 0f; // Reset the damage timer
            }
        }
    }

    internal bool IsDead()
    {
        return _isDead;
    }
}
