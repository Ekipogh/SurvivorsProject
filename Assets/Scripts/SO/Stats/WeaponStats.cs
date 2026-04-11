using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [FormerlySerializedAs("damageModifier")]
    public float DamageModifier = 10f; // Damage modifier for the weapon
    [FormerlySerializedAs("rotationSpeed")]
    public float RotationSpeed = 50f; // Speed of rotation towards the target
    [FormerlySerializedAs("range")]
    public float Range = 5f; // Range of the weapon
    [FormerlySerializedAs("attackAngle")]
    public float AttackAngle = 45f; // Angle of attack cone
    [FormerlySerializedAs("aimTolerance")]
    public float AimTolerance = 6f; // Angle threshold required to fire at the target
    [FormerlySerializedAs("cooldownTime")]
    public float CooldownTime = 1f; // Cooldown time between attacks

    [FormerlySerializedAs("projectileSpeed")]
    public float ProjectileSpeed = 20f; // Speed of the projectile
    [FormerlySerializedAs("aoeRadius")]
    public float AoeRadius = 2f; // Radius of area of effect for explosive weapons
}
