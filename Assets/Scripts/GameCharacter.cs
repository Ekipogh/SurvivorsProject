using UnityEngine;
using UnityEngine.Serialization;

public abstract class GameCharacter : MonoBehaviour
{
    [FormerlySerializedAs("characterStats")]
    public CharacterStats Stats;
    private Microlight.MicroBar.MicroBar _healthBar; // Reference to the health bar

    private HPBar _hpBarInstance; // Reference to the HP bar instance

    protected virtual void Awake()
    {
        Stats.CurrentHealth = Stats.MaxHealth;
        // spawn the health bar
        HPBar hpBarPrefab = Resources.Load<HPBar>("Prefabs/HPBar");
        if (hpBarPrefab != null)
        {
            _hpBarInstance = Instantiate(hpBarPrefab);
            _hpBarInstance.Target = transform; // Set the target of the HP bar to this character
            _healthBar = _hpBarInstance.GetComponentInChildren<Microlight.MicroBar.MicroBar>(); // Get the MicroBar component from the HP bar instance
            if (_healthBar != null)
            {
                _healthBar.Initialize(Stats.MaxHealth); // Initialize the health bar with the max health of the character
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        Stats.CurrentHealth -= damage;
        if (Stats.CurrentHealth <= 0)
        {
            Die();
        }
        if (_healthBar != null)
        {
            _healthBar.UpdateBar(Stats.CurrentHealth, Microlight.MicroBar.UpdateAnim.Damage); // Update the health bar with damage animation
        }
    }

    void OnDestroy()
    {
        // Clean up the HP bar instance when the character is destroyed
        if (_hpBarInstance != null)
        {
            Destroy(_hpBarInstance.gameObject);
        }
    }

    protected abstract void Die();
}
