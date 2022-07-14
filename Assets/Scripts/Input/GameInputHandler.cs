using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction _move;
    
    public Vector2 RawMoveInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public JumpInputInfo JumpInput { get; private set; }
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions["Move"];

        JumpInput = new JumpInputInfo();
    }

    private void Start()
    {
        _playerInput.onActionTriggered += OnMoveInput;
        _playerInput.onActionTriggered += OnJumpInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.action != _move) return;
        RawMoveInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(RawMoveInput.x);
        NormInputY = Mathf.RoundToInt(RawMoveInput.y);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Jump") return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                JumpInput.press = true;
                break;
            case InputActionPhase.Performed:
                JumpInput.hold = true;
                break;
            case InputActionPhase.Canceled:
                JumpInput.press = false;
                JumpInput.hold = false;
                break;
        }
    }
}

public sealed class JumpInputInfo
{
    public bool press;
    public bool hold;
}
