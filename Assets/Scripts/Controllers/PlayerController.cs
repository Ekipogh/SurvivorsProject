using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Player player;
    private bool _isGamepad = false;

    void OnMove(InputValue value)
    {
        if (player == null)
        {
            return;
        }

        var moveInput = value.Get<Vector2>();
        player.Move(moveInput);
    }

    void OnLook(InputValue value)
    {
        if (player == null)
        {
            return;
        }

        var lookInput = value.Get<Vector2>();
        if (_isGamepad)
        {
            if (lookInput.sqrMagnitude > player.stats.lookInputDeadzone * player.stats.lookInputDeadzone)
            {
                player.LookTowards(lookInput);
            }
        }
    }

    void OnRotate(InputValue value)
    {
        if (player == null)
        {
            return;
        }

        player.Rotate(value.Get<float>());
    }

    void OnControlsChanged(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme == "Gamepad";
    }
}
