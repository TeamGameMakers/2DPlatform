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

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions["Move"];
    }

    private void Start()
    {
        _playerInput.onActionTriggered += OnMoveInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.action != _move) return;
        RawMoveInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(RawMoveInput.x);
        NormInputY = Mathf.RoundToInt(RawMoveInput.y);
    }
}
