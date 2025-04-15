using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 5f;
    public float rotationSpeed = 100f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool _isGamepad = false;

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnControlsChanged(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme == "Gamepad";
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        var movement = moveInput * speed * Time.deltaTime;
        if (movement != Vector2.zero)
        {
            playerTransform.Translate(movement, Space.World);
        }
    }

    void RotatePlayer()
    {
        if (_isGamepad)
        {
            if (lookInput.magnitude > 0.1f)
            {
                LookTowards(lookInput);
            }
        }
        else
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane));
            mousePosition.z = 0; // Ensure we are only rotating in the XY plane
            var direction = (mousePosition - playerTransform.position).normalized;
            direction.z = 0; // Ensure we are only rotating in the XY plane
            if (direction != Vector3.zero)
            {
                LookTowards(direction);
            }
        }
    }

    private void LookTowards(Vector2 direction){
        if (direction != Vector2.zero)
        {
            var angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        }
    }
}
