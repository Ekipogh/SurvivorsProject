using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : GameCharacter
{
    private Rigidbody2D _rb;

    private readonly Dictionary<string, float> _speedBonuses = new();
    private readonly Dictionary<string, float> _speedMultipliers = new();

    private Vector2 _currentVelocity;
    private Vector2 _lookDirection;
    private Vector2 _moveInput;
    private float _rotationInput;
    private bool _hasLookDirection;

    [FormerlySerializedAs("playerStats")]
    public PlayerStats PlayerStatsData;

    [FormerlySerializedAs("weapons")]
    public List<Weapon> Weapons = new();

    public Rigidbody2D Rb
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


    protected override void Awake()
    {
        _currentVelocity = Vector2.zero;
        _lookDirection = Vector2.zero;
        _moveInput = Vector2.zero;
        _rotationInput = 0f;
        _hasLookDirection = false;
        base.Awake();

        // reset points to 0 at the start of the game
        if (PlayerStatsData != null)        {
            PlayerStatsData.CurrentPoints = 0f;
        }
    }

    public void Move(Vector2 direction)
    {
        _moveInput = direction;
    }

    public void LookTowards(Vector2 direction)
    {
        if (direction.sqrMagnitude <= Stats.LookInputDeadzone * Stats.LookInputDeadzone)
        {
            return;
        }

        _lookDirection = direction.normalized;
        _hasLookDirection = true;
    }

    public void Rotate(float direction)
    {
        _rotationInput = Mathf.Clamp(direction, -1f, 1f);
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector2 desiredInput = _moveInput;
        if (desiredInput.sqrMagnitude <= Stats.MoveInputDeadzone * Stats.MoveInputDeadzone)
        {
            desiredInput = Vector2.zero;
        }
        else if (desiredInput.sqrMagnitude > 1f)
        {
            desiredInput.Normalize();
        }

        float modifiedSpeed = GetModifiedSpeed();
        Vector2 targetVelocity = desiredInput * modifiedSpeed;
        float acceleration = desiredInput == Vector2.zero ? Stats.Deceleration : Stats.Acceleration;
        _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        if (_currentVelocity.sqrMagnitude <= 0.0001f)
        {
            _currentVelocity = Vector2.zero;
        }

        Rb.MovePosition(Rb.position + _currentVelocity * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        if (!Mathf.Approximately(_rotationInput, 0f))
        {
            float nextAngle = Rb.rotation - _rotationInput * Stats.RotationSpeed * Time.fixedDeltaTime;
            Rb.MoveRotation(nextAngle);
            return;
        }

        if (_hasLookDirection)
        {
            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 90f;
            float nextAngle = Mathf.MoveTowardsAngle(Rb.rotation, angle, Stats.RotationSpeed * Time.fixedDeltaTime);
            Rb.MoveRotation(nextAngle);
        }
    }

    public void SetSpeedMultiplier(string sourceId, float multiplier)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
        {
            return;
        }

        _speedMultipliers[sourceId] = Mathf.Max(0f, multiplier);
    }

    public void RemoveSpeedMultiplier(string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
        {
            return;
        }

        _speedMultipliers.Remove(sourceId);
    }

    public void SetSpeedBonus(string sourceId, float bonus)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
        {
            return;
        }

        _speedBonuses[sourceId] = bonus;
    }

    public void RemoveSpeedBonus(string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
        {
            return;
        }

        _speedBonuses.Remove(sourceId);
    }

    public Vector2 CurrentVelocity => _currentVelocity;

    public float CurrentMoveSpeed => GetModifiedSpeed();

    protected override void Die()
    {
        Debug.Log("Player has died.");
    }

    public void UpdateEnemyList(List<Enemy> enemyList)
    {
        foreach (Weapon weapon in Weapons)
        {
            weapon.EnemyList = enemyList;
        }
    }

    private float GetModifiedSpeed()
    {
        float speed = Mathf.Max(0f, Stats.Speed);

        foreach (float bonus in _speedBonuses.Values)
        {
            speed += bonus;
        }

        foreach (float multiplier in _speedMultipliers.Values)
        {
            speed *= Mathf.Max(0f, multiplier);
        }

        return Mathf.Max(0f, speed);
    }

    public void RewardPoints(float points)
    {
        PlayerStatsData.CurrentPoints += points;
    }
}
