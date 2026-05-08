using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputActions _inputActions;

    public event Action OnRMB;
    public Vector2 MoveInput { get; private set; }
    public Vector2 PointerPosition { get; private set; }

    private void Awake()
    {
        _inputActions = new InputActions();
        Enable();
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

    public void Disable()
    {
        _inputActions.Disable();
    }
}