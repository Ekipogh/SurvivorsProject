using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float speed = 5f;
    public float acceleration = 30f;
    public float deceleration = 40f;
    public float moveInputDeadzone = 0.1f;
    public float lookInputDeadzone = 0.1f;
    public float rotationSpeed = 540f;
    public float maxHealth = 100f;

    public float currentHealth = 100f; // Current health of the character
}
