using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    private float _speed = 2.5f;
    private float _rotationSpeed = 100f;

    private bool _isDamagingPlayer = false;

    void Update()
    {
        // Move towards the player
        if (player != null && !_isDamagingPlayer)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += _speed * Time.deltaTime * (Vector3)direction;
            // rotate towards the player
            Vector3 targetDirection = player.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle collision with the player
            Debug.Log("Enemy collided with the player!");
            // You can add logic here to deal damage or trigger other effects
            _isDamagingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle exit from collision with the player
            Debug.Log("Enemy exited collision with the player!");
            // Reset the damaging state when the player exits the trigger
            _isDamagingPlayer = false;
        }
    }
}
