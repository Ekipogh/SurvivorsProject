using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Get the Enemy component from the collided object
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                Debug.Log("Hit enemy: " + enemy.name);
                // Deal damage to the enemy
                enemy.TakeDamage(10f); // Replace 10f with the actual damage value
            }

            // Destroy the bullet after it hits an enemy
            Destroy(gameObject);
        }
    }
}
