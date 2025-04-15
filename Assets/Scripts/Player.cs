using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;


    private float _speed = 5f;
    private const float _defaultSpeed = 5f;

    private float _rotationSpeed = 100f;
    private const float _defaultRotationSpeed = 100f;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;

    public Rigidbody2D rb
    {
        get
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody2D>();
            }
            return _rb;
        }
    }

    public float speed
    {
        get { return _speed * 100 / _defaultSpeed; }
        set { _speed = value * _defaultSpeed / 100; }
    }

    public float rotationSpeed
    {
        get { return _rotationSpeed * 100 / _defaultRotationSpeed; }
        set { _rotationSpeed = value * _defaultRotationSpeed / 100; }
    }

    private void Awake()
    {
        _moveDirection = Vector2.zero;
        _lookDirection = Vector2.zero;
    }

    public void Move(Vector2 direction)
    {
        _moveDirection = direction;
    }

    public void LookTowards(Vector2 direction)
    {
        _lookDirection = direction;
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        _moveDirection = _moveDirection.normalized * _speed * Time.deltaTime;
        rb.MovePosition(rb.position + _moveDirection);
    }

    private void RotatePlayer()
    {
        if (_lookDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, _rotationSpeed * Time.deltaTime));
        }
    }
}
