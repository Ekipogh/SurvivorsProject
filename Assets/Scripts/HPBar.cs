using UnityEngine;

public class HPBar : MonoBehaviour
{
    private Transform _target;

    private float _offsetY = 0.2f;

    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    void Awake()
    {
        // uncouple the HP bar from the target so it doesn't inherit any transformations
        transform.SetParent(null, true);
    }

    void LateUpdate()
    {
        // keep the HP bar above the target and facing the camera
        if (_target != null)
        {
            transform.position = _target.position + Vector3.down * _offsetY;
        }
        transform.rotation = Quaternion.identity;
    }
}
