using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float DamageModifier = 10f; // Damage modifier for the weapon
    public float RotationSpeed = 50f; // Speed of rotation towards the target
    public float Range = 5f; // Range of the weapon
    public float AttackAngle = 45f; // Angle of attack cone
    public float AimTolerance = 6f; // Angle threshold required to fire at the target
    public float CooldownTime = 1f; // Cooldown time between attacks
    public float ProjectileSpeed = 20f; // Speed of the projectile
    public float AoeRadius = 2f; // Radius of area of effect for explosive weapons
}
