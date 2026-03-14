using System.Collections.Generic;
using UnityEngine;

public class Player : GameCharacter
{
    private Rigidbody2D _rb;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;

    public List<Weapon> weapons = new List<Weapon>();

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
        _moveDirection = Vector2.zero;
        _lookDirection = Vector2.zero;
        base.Awake();
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
        _moveDirection = _moveDirection.normalized * stats.speed * Time.deltaTime;
        Rb.MovePosition(Rb.position + _moveDirection);
    }

    private void RotatePlayer()
    {
        if (_lookDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 90f;
            Rb.MoveRotation(Mathf.LerpAngle(Rb.rotation, angle, stats.rotationSpeed * Time.deltaTime));
        }
    }

    protected override void Die()
    {
        Debug.Log("Player has died.");
    }

    public void UpdateEnemyList(List<Enemy> enemyList)
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.enemyList = enemyList;
        }
    }
}
