using UnityEngine;

public class SimpleHommingRocket : MonoBehaviour
{
    public Transform target; // The target the rocket will follow
    public float speed = 5f; // Speed of the rocket
    public float rotationSpeed = 200f; // Rotation speed of the rocket
    public float damage = 20f; // Damage dealt by the rocket
    public float explosionRadius = 2f; // Radius of the explosion effect

    public GameObject explosionEffect; // Prefab for explosion effect

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // find all enemies within the explosion radius and apply damage
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                if (hitEnemy.CompareTag("Enemy"))
                {
                    if (hitEnemy.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.TakeDamage(damage); // Apply damage to the enemy
                    }
                }
            }
            Instantiate(explosionEffect, transform.position, Quaternion.identity); // Create explosion effect
            Destroy(gameObject); // Destroy the rocket after hitting an enemy
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();

            // Rotate the rocket towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the rocket forward
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            // If there is no target, move straight forward
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}
