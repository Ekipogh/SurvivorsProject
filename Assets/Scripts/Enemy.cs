using System;
using Microlight.MicroBar;
using UnityEngine;

public class Enemy : GameCharacter
{
    public Player player;

    public EnemyStats enemyStats;

    // States
    private bool _isDamagingPlayer = false;
    private bool _isDead = false;

    private float _damageCooldown = 1f; // damage cooldown time in seconds
    private float _damageTimer = 2f; // timer to track damage cooldown, start able to damage player

    public Vector3 rotatation;

    public Transform sprite;

    void Update()
    {
        // Move towards the player
        if (player != null && !_isDamagingPlayer)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += stats.speed * Time.deltaTime * (Vector3)direction;
            // rotate towards the player
            Vector3 targetDirection = player.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, rotation, stats.rotationSpeed * Time.deltaTime);
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
    }

    private void DamagePlayer()
    {
        if (_isDamagingPlayer && player != null)
        {
            // Check if the damage cooldown has elapsed
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= _damageCooldown)
            {
                // Deal damage to the player
                player.TakeDamage(enemyStats.damage);
                _damageTimer = 0f; // Reset the damage timer
            }
        }
    }

    internal bool IsDead()
    {
        return _isDead;
    }
}
