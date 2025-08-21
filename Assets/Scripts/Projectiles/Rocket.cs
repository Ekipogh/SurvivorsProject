using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab;
    private float explosionRadius = 5f;
    private float fuseTime = 3f;

    public List<Enemy> enemies;

    void OnTriggerEnter(Collider other)
    {
        // if hit an enemy or an object
        if (other.CompareTag("Enemy") || other.CompareTag("CollidableObject"))
        {
            // spawn a explosion effect
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // damage enemies in radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
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
