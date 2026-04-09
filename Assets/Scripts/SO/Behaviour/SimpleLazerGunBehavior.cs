using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleLazerGunBehavior", menuName = "Scriptable Objects/SimpleLazerGunBehavior")]
public class SimpleLazerGunBehavior : WeaponBehaviour
{
    public Sprite lazerSprite; // Sprite for the laser
    private const float laserWidth = 0.1f; // Width of the laser line
    private readonly Color laserColor = Color.red; // Color of the laser
    public override void Shoot(Weapon weapon)
    {
        if (weapon.TargetEnemy == null) return;

        GameObject laserGO = new("LaserLine");
        LineRenderer lr = laserGO.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, weapon.ProjectileSpawnPosition);
        lr.SetPosition(1, weapon.TargetEnemy.transform.position);

        lr.startWidth = laserWidth;
        lr.endWidth = laserWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = laserColor;
        lr.endColor = laserColor;

        var damage = weapon.stats.damageModifier;
        weapon.TargetEnemy.TakeDamage(damage);

        weapon.StartCoroutine(FadeOutLaser(lr, 0.2f));

    }

    private IEnumerator FadeOutLaser(LineRenderer lineRenderer, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = lineRenderer.startColor;
        Color endColor = new(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            lineRenderer.startColor = Color.Lerp(startColor, endColor, elapsedTime / duration);
            lineRenderer.endColor = lineRenderer.startColor;
            yield return null;
        }

        lineRenderer.enabled = false;
    }
}
