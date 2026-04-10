using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    public CharacterStats stats;
    private Microlight.MicroBar.MicroBar healthBar; // Reference to the health bar

    private HPBar hpBarInstance; // Reference to the HP bar instance

    protected virtual void Awake()
    {
        stats.currentHealth = stats.maxHealth;
        // spawn the health bar
        HPBar hpBarPrefab = Resources.Load<HPBar>("Prefabs/HPBar");
        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab);
            hpBarInstance.Target = transform; // Set the target of the HP bar to this character
            healthBar = hpBarInstance.GetComponentInChildren<Microlight.MicroBar.MicroBar>(); // Get the MicroBar component from the HP bar instance
            if (healthBar != null)
            {
                healthBar.Initialize(stats.maxHealth); // Initialize the health bar with the max health of the character
            }
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

    void OnDestroy()
    {
        // Clean up the HP bar instance when the character is destroyed
        if (hpBarInstance != null)
        {
            Destroy(hpBarInstance.gameObject);
        }
    }

    protected abstract void Die();
}
