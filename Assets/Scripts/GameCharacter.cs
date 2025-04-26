using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    public CharacterStats stats;
    protected float _currentHealth;

    protected virtual void Awake()
    {
        _currentHealth = stats.maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
