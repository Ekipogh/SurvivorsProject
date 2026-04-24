using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SimpleLazerGunBehavior", menuName = "Scriptable Objects/SimpleLazerGunBehavior")]
public class SimpleLazerGunBehavior : WeaponBehaviour
{
    public Sprite LaserSprite; // Sprite for the laser
    private const float LaserWidth = 0.1f; // Width of the laser line
    private readonly Color _laserColor = Color.red; // Color of the laser
    public override void Shoot(Weapon weapon)
    {
        if (weapon.TargetEnemy == null) return;

        float range = weapon.Stats.Range;
        float damage = weapon.Stats.DamageModifier;
        int maxPenetrations = weapon.Stats.MaxPenetrations;

        // Perform raycast from firing point in aim direction
        RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.ProjectileSpawnPosition, weapon.AimDirection, range);

        // Track which enemies have been damaged to apply penetration limit
        int penetrationCount = 0;
        foreach (RaycastHit2D hit in hits)
        {
            // Check if we've exceeded penetration limit
            if (maxPenetrations >= 0 && penetrationCount >= maxPenetrations)
            {
                break;
            }

            // Check if the hit collider is an enemy
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(damage);
                    penetrationCount++;
                }
            }
        }

        // Draw visual laser line from firing point to target position
        GameObject laserGO = new("LaserLine");
        LineRenderer lr = laserGO.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, weapon.ProjectileSpawnPosition);
        lr.SetPosition(1, weapon.TargetEnemy.transform.position);

        lr.startWidth = LaserWidth;
        lr.endWidth = LaserWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = _laserColor;
        lr.endColor = _laserColor;

        weapon.StartCoroutine(FadeOutLaser(laserGO, 0.2f));
    }

    private IEnumerator FadeOutLaser(GameObject laserGO, float duration)
    {
        LineRenderer lineRenderer = laserGO.GetComponent<LineRenderer>();
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

        Destroy(laserGO);
    }
}
