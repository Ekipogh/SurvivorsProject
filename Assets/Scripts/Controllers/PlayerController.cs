using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("player")]
    public Player Player;
    private bool _isGamepad = false;

    void OnMove(InputValue value)
    {
        if (Player == null)
        {
            return;
        }

        var moveInput = value.Get<Vector2>();
        Player.Move(moveInput);
    }

    void OnLook(InputValue value)
    {
        if (Player == null)
        {
            return;
        }

        var lookInput = value.Get<Vector2>();
        if (_isGamepad)
        {
            if (lookInput.sqrMagnitude > Player.Stats.LookInputDeadzone * Player.Stats.LookInputDeadzone)
            {
                Player.LookTowards(lookInput);
            }
        }
    }

    void OnRotate(InputValue value)
    {
        if (Player == null)
        {
            return;
        }

        Player.Rotate(value.Get<float>());
    }

    void OnControlsChanged(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme == "Gamepad";
    }
}
