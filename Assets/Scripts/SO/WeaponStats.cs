using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float damageModifier = 10f; // Damage modifier for the weapon
    public float rotationSpeed = 50f; // Speed of rotation towards the target
    public float range = 5f; // Range of the weapon
    public float attackAngle = 45f; // Angle of attack cone
    public float cooldownTime = 1f; // Cooldown time between attacks

    public float bulletSpeed = 20f; // Speed of the bullet
}
