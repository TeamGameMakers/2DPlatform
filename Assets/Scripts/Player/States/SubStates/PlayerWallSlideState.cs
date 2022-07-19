using Base.FSM;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetNumOfJump();
    }

    public override void Exit()
    {
        base.Exit();
        core.Movement.Flip(-core.Movement.FaceDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (core.Detection.grounded)
            stateMachine.ChangeState(player.IdleState);

        if (InputX * core.Movement.FaceDirection < 0 || !core.Detection.touchWall)
        {
            player.JumpState.DecreaseNumOfJump();
            stateMachine.ChangeState(player.AirState);
        }

        if (JumpInput.Press)
        {
            JumpInput.UseJumpInput();
            stateMachine.ChangeState(player.WallJumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        core.Movement.SetFriction(0);
        core.Movement.SetVelocityX(core.Movement.FaceDirection * 3);
        core.Movement.SetVelocityY(-data.wallSlideVelocity);
    }
}
