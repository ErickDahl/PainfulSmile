using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IGameplayActions
{
    private PlayerInput _playerInput;

    public event Action<bool> OnShootFrontEvent;
    public event Action<bool> OnShootSideEvent;
    public event Action<Vector2> OnMoveEvent;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.Gameplay.SetCallbacks(this);
            _playerInput.Gameplay.Enable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShootFront(InputAction.CallbackContext context)
    {
        if (
            context.phase == InputActionPhase.Performed
            || context.phase == InputActionPhase.Canceled
        )
        {
            OnShootFrontEvent?.Invoke(ConvertNumberToBool(context.ReadValue<float>()));
        }
    }

    public void OnShootSide(InputAction.CallbackContext context)
    {
        if (
            context.phase == InputActionPhase.Performed
            || context.phase == InputActionPhase.Canceled
        )
        {
            OnShootSideEvent?.Invoke(ConvertNumberToBool(context.ReadValue<float>()));
        }
    }

    private bool ConvertNumberToBool(float value)
    {
        return value > 0;
    }
}
