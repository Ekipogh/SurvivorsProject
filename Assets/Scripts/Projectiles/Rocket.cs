using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    private float _explosionRadius = 5f;
    private float _fuseTime = 3f;

    public List<Enemy> Enemies;

    void OnTriggerEnter(Collider other)
    {
        // if hit an enemy or an object
        if (other.CompareTag("Enemy") || other.CompareTag("CollidableObject"))
        {
            // spawn a explosion effect
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            // damage enemies in radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    hitCollider.GetComponent<Enemy>().TakeDamage(50f);
                }
            }
        }
    }
}
