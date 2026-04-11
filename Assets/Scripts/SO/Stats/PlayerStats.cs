using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [FormerlySerializedAs("currentPoints")]
    public float CurrentPoints = 0f; // Current points of the player
}
