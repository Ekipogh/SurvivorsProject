using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    public CharacterStats stats;

    protected virtual void Awake()
    {
        stats.currentHealth = stats.maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
