using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public float maxHealth = 100f;
}
