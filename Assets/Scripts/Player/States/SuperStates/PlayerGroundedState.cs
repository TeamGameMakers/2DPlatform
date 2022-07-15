using Base.FSM;
using UnityEngine;

public class PlayerGroundedState: PlayerState
{
    protected PlayerGroundedState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        
        player.JumpState.ResetNumOfJump();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!player.CollDetector.onGround)
        {
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        }

        if (JumpInput.Press && player.JumpState.CanJump)
        {
            JumpInput.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}