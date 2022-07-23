using Base.FSM;
using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _numOfJump;

    public bool CanJump => _numOfJump > 0;

    public PlayerJumpState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) :
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _numOfJump--;
        if (core.Detection.touchWall)
        {
            player.InputHandler.LockMoveInputX(0);
            player.StartCoroutine(CloseWallJump());
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetVelocityY(data.jumpVelocity);
        player.AirState.StartJumping();
        isAbilityDone = true;
    }

    private IEnumerator CloseWallJump()
    {
        while (core.Movement.CurrentVelocity.y > -0.01f && player.InputHandler.MoveInputLock)
        {
            yield return null;
        }
        
        player.InputHandler.UnLockMoveInputX();
    }

    public void ResetNumOfJump() => _numOfJump = data.numOfJump;
    public void DecreaseNumOfJump() => _numOfJump--;
}
