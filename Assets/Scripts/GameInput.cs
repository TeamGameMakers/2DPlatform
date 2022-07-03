using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public float moveInput;
    public event Action JumpStartEvent;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += GetMoveInput;
        _playerInput.onActionTriggered += GetJumpInput;
    }

    private void GetMoveInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move") return;
        moveInput = context.ReadValue<float>();
    }

    private void GetJumpInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Jump") return;
        
        if (context.started)
        {
            JumpStartEvent?.Invoke();
        }

        else if (context.performed)
        {
            //TODO: 长按大跳
        }
    }
}