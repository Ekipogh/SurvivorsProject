using UnityEngine;

[CreateAssetMenu(fileName = "SimpleLazerGunBehavior", menuName = "Scriptable Objects/SimpleLazerGunBehavior")]
public class SimpleLazerGunBehavior : ProjectileBehaviour
{
    public Sprite lazerSprite; // Sprite for the laser
    public override void Shoot(Weapon weapon)
    {
        var shootDirection = weapon.transform.up;
        // TODO: When lazer shoots, draw a lazer line that lingers for a while
        // TODO: Apply damage to the target enemy, if the lazer hits the enemy
        // Draw the laser line
        LineRenderer lineRenderer = weapon.firingPoint.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, weapon.firingPoint.position);
        lineRenderer.SetPosition(1, weapon.firingPoint.position + shootDirection * weapon.stats.range); // Adjust length as needed
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingLayerName = "Foreground"; // Set the sorting layer to "Foreground"
        lineRenderer.sortingOrder = 1; // Set the sorting order to 1 (higher than default)
        lineRenderer.startLifetime = 0.1f; // Duration of the laser effect
        lineRenderer.endLifetime = 0.1f; // Duration of the laser effect
        lineRenderer.enabled = true; // Enable the line renderer to show the laser

    }
}
