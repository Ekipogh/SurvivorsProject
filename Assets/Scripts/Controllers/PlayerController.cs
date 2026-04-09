using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Player player;

    private Camera _mainCamera;
    private Vector2 _mouseScreenPosition;
    private bool _hasMouseScreenPosition;

    private bool _isGamepad = false;

    private Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            return _mainCamera;
        }
    }

    private void Update()
    {
        if (!_isGamepad)
        {
            UpdateMouseLook();
        }
    }

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
            if (lookInput.sqrMagnitude > player.stats.lookInputDeadzone * player.stats.lookInputDeadzone)
            {
                player.LookTowards(lookInput);
            }
        }
        else
        {
            _mouseScreenPosition = lookInput;
            _hasMouseScreenPosition = true;
        }
    }

    void OnControlsChanged(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme == "Gamepad";
    }

    private void UpdateMouseLook()
    {
        if (player == null || MainCamera == null)
        {
            return;
        }

        if (Mouse.current != null)
        {
            _mouseScreenPosition = Mouse.current.position.ReadValue();
            _hasMouseScreenPosition = true;
        }

        if (!_hasMouseScreenPosition)
        {
            return;
        }

        Vector3 mousePosition = MainCamera.ScreenToWorldPoint(new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, Mathf.Abs(MainCamera.transform.position.z)));
        mousePosition.z = 0f;

        Vector2 direction = mousePosition - player.transform.position;
        player.LookTowards(direction);
    }
}
