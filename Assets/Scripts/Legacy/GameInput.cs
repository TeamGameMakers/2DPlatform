 using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Legacy
{
    [RequireComponent(typeof(PlayerInput))]
    public class GameInput : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputAction _move;

        public float moveInput;
        public bool canMove = true;
        public event Action JumpStartEvent;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.onActionTriggered += GetMoveInput;
            _playerInput.onActionTriggered += GetJumpInput;
        
            //记录 Action
            _move = _playerInput.currentActionMap.FindAction("Move");
        }

        private void GetMoveInput(InputAction.CallbackContext context)
        {
            if (context.action.name != "Move" || !canMove) return;
        
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
    
        /// <summary>
        /// 手动更新数值
        /// </summary>
        public void UpdateMoveInput()
        {
            if (_move.inProgress)
            {
                moveInput = _move.ReadValue<float>();
            }
        }
    }
}