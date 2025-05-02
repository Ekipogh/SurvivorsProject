using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    public CharacterStats stats;
    public Microlight.MicroBar.MicroBar healthBar; // Reference to the health bar

    protected virtual void Awake()
    {
        stats.currentHealth = stats.maxHealth;
        if (healthBar != null)
        {
            healthBar.Initialize(stats.maxHealth); // Initialize the health bar
        }
    }

    public virtual void TakeDamage(float damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            Die();
        }
        if (healthBar != null)
        {
            healthBar.UpdateBar(stats.currentHealth, Microlight.MicroBar.UpdateAnim.Damage); // Update the health bar with damage animation
        }
    }

    protected abstract void Die();
}
