using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float Speed = 5f;
    public float Acceleration = 30f;
    public float Deceleration = 40f;
    public float MoveInputDeadzone = 0.1f;
    public float LookInputDeadzone = 0.1f;
    public float RotationSpeed = 540f;
    public float MaxHealth = 100f;

    public float CurrentHealth = 100f; // Current health of the character
}
