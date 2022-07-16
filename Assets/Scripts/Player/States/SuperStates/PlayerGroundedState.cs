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

        if (!core.Detection.grounded)
        {
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        }

        if (JumpInput.Press && player.JumpState.CanJump && !core.Detection.onSteep)
        {
            JumpInput.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        
        if (core.Detection.onSteep)
            core.Movement.SetFriction(0.3f);
        else
            core.Movement.SetFriction(0.3f);
    }
}