using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Player player;


    private bool _isGamepad = false;

    void OnMove(InputValue value)
    {
        var moveInput = value.Get<Vector2>();
        player.Move(moveInput);
    }

    void OnLook(InputValue value)
    {
        var lookInput = value.Get<Vector2>();
        if (_isGamepad)
        {
            if (lookInput.magnitude > 0.1f)
            {
                player.LookTowards(lookInput);
            }
        }
        else
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(lookInput.x, lookInput.y, Camera.main.nearClipPlane));
            mousePosition.z = 0; // Ensure we are only rotating in the XY plane
            var direction = (mousePosition - player.transform.position).normalized;
            direction.z = 0; // Ensure we are only rotating in the XY plane
            lookInput = direction;
            player.LookTowards(lookInput);
        }
    }

    void OnControlsChanged(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme == "Gamepad";
    }
}
