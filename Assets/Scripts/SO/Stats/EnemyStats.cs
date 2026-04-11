using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [FormerlySerializedAs("damage")]
    public float Damage = 10f; // Damage dealt to the player
    [FormerlySerializedAs("pointsValue")]
    public float PointsValue = 100f; // Points awarded to the player when this enemy is defeated
}
