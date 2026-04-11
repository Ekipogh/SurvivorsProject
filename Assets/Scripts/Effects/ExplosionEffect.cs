using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionEffect : MonoBehaviour
{
    [FormerlySerializedAs("duration")]
    public float Duration = 1f; // Duration of the explosion effect

    // Update is called once per frame
    void Update()
    {
        Duration -= Time.deltaTime;
        if (Duration <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
