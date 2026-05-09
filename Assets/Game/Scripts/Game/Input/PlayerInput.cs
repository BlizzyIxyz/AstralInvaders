using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputActions _inputActions;

    public event Action OnRMB;
    public bool IsLMBPressed { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 PointerPosition { get; private set; }

    private void Awake()
    {
        _inputActions = new InputActions();
        RegisterHandlers();
    }

    public void Enable()
    {
        _inputActions.Enable();
    }

    private void RegisterHandlers()
    {
        _inputActions.Player.Move.performed += HandleMovePerformed;
        _inputActions.Player.Move.canceled += HandleMoveCancelled;

        _inputActions.Player.PointerMove.performed += HandlePointerMovePerformed;

        _inputActions.Player.RMB.started += HandleRMBStarted;

        _inputActions.Player.LMB.started += HandleLMBStarted;
        _inputActions.Player.LMB.canceled += HandleLMBCancelled;
    }

    private void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void HandleMoveCancelled(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void HandlePointerMovePerformed(InputAction.CallbackContext ctx)
    {
        PointerPosition = ctx.ReadValue<Vector2>();
    }

    private void HandleRMBStarted(InputAction.CallbackContext _)
    {
        OnRMB?.Invoke();
    }

    private void HandleLMBStarted(InputAction.CallbackContext ctx)
    {
        IsLMBPressed = true;
    }

    private void HandleLMBCancelled(InputAction.CallbackContext ctx)
    {
        IsLMBPressed = false;
    }

    public void Disable()
    {
        _inputActions.Disable();
        IsLMBPressed = false;
    }
}