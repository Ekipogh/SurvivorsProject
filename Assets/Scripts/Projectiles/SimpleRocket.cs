using UnityEngine;
using UnityEngine.Serialization;

public class SimpleHommingRocket : MonoBehaviour
{
    public Transform Target; // The target the rocket will follow
    public float Speed = 5f; // Speed of the rocket
    public float RotationSpeed = 200f; // Rotation speed of the rocket
    public float Damage = 20f; // Damage dealt by the rocket
    public float ExplosionRadius = 2f; // Radius of the explosion effect

    public GameObject ExplosionEffect; // Prefab for explosion effect

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // find all enemies within the explosion radius and apply damage
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                if (hitEnemy.CompareTag("Enemy"))
                {
                    if (hitEnemy.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.TakeDamage(Damage); // Apply damage to the enemy
                    }
                }
            }
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity); // Create explosion effect
            Destroy(gameObject); // Destroy the rocket after hitting an enemy
        }
    }

    void Update()
    {
        if (Target != null)
        {
            // Calculate the direction to the target
            Vector2 direction = (Vector2)Target.position - (Vector2)transform.position;
            direction.Normalize();

            // Rotate the rocket towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

            // Move the rocket forward
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        else
        {
            // If there is no target, move straight forward
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
    }
}
