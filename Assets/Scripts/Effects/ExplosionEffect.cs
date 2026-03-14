using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float duration = 1f; // Duration of the explosion effect

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
