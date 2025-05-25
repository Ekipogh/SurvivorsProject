using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleLazerGunBehavior", menuName = "Scriptable Objects/SimpleLazerGunBehavior")]
public class SimpleLazerGunBehavior : ProjectileBehaviour
{
    public Sprite lazerSprite; // Sprite for the laser
    public override void Shoot(Weapon weapon)
    {
        if (weapon.TargetEnemy == null) return;

        // Create a new GameObject for the laser line
        GameObject laserGO = new GameObject("LaserLine");
        LineRenderer lr = laserGO.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, weapon.firingPoint.position);
        lr.SetPosition(1, weapon.TargetEnemy.transform.position);
        // Optionally set material, color, width, etc.

        var damage = weapon.stats.damageModifier;
        // Apply instant damage
        weapon.TargetEnemy.TakeDamage(weapon.stats.damageModifier);

        // Start coroutine to fade and destroy the laser
        weapon.StartCoroutine(FadeOutLaser(lr, 0.2f));

    }

    private IEnumerator FadeOutLaser(LineRenderer lineRenderer, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = lineRenderer.startColor;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            lineRenderer.startColor = Color.Lerp(startColor, endColor, elapsedTime / duration);
            lineRenderer.endColor = lineRenderer.startColor;
            yield return null;
        }

        // Optionally disable the LineRenderer after fading out
        lineRenderer.enabled = false;
    }
}
