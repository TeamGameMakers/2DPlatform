using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public Vector2 moveInput;
    public JumpInput jumpInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += GetMoveInput;
    }

    private void GetMoveInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move") return;
        moveInput = context.ReadValue<Vector2>();
    }

    private void GetJumpInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Jump") return;
        
        if (context.started)
        {
            jumpInput.press = true;
        }

        else if (context.performed)
        {
            jumpInput.hold = true;
        }
            
        else if (context.canceled)
        {
            jumpInput.hold = false;
            jumpInput.press = false;
        }
    }
}

[Serializable]
public struct JumpInput
{
    /// <summary>
    /// 按住跳跃键
    /// </summary>
    public bool hold;
    
    /// <summary>
    /// 按下跳跃键
    /// </summary>
    public bool press;
}