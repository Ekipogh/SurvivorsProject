using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [FormerlySerializedAs("speed")]
    public float Speed = 5f;
    [FormerlySerializedAs("acceleration")]
    public float Acceleration = 30f;
    [FormerlySerializedAs("deceleration")]
    public float Deceleration = 40f;
    [FormerlySerializedAs("moveInputDeadzone")]
    public float MoveInputDeadzone = 0.1f;
    [FormerlySerializedAs("lookInputDeadzone")]
    public float LookInputDeadzone = 0.1f;
    [FormerlySerializedAs("rotationSpeed")]
    public float RotationSpeed = 540f;
    [FormerlySerializedAs("maxHealth")]
    public float MaxHealth = 100f;

    [FormerlySerializedAs("currentHealth")]
    public float CurrentHealth = 100f; // Current health of the character
}
