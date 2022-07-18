using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction _move;
    private bool _moveInputLock;
    
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
        if (context.action != _move || _moveInputLock) return;
        RawMoveInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(RawMoveInput.x);
        NormInputY = Mathf.RoundToInt(RawMoveInput.y);
    }

    public void LockMoveInputX(int value)
    {
        _moveInputLock = true;
        NormInputX = value;
    }

    public void UnLockMoveInputX()
    {
        _moveInputLock = false;
        NormInputX = Mathf.RoundToInt(_move.ReadValue<Vector2>().x);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Jump") return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                JumpInput.press = true;
                JumpInput.hold = false;
                break;
            case InputActionPhase.Performed:
                JumpInput.hold = true;
                break;
            case InputActionPhase.Canceled:
                JumpInput.hold = false;
                JumpInput.press = false;
                break;
        }
    }
}

public sealed class JumpInputInfo
{
    internal bool press;
    internal bool hold;

    public bool Press => press;
    public bool Hold => hold;
    
    public void UseJumpInput() => press = false;
}
