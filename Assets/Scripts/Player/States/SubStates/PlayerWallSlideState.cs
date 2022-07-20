using Base.FSM;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private float timer;
    public bool Sliding { get; private set; }

    public PlayerWallSlideState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetNumOfJump();
        timer = 0.2f;
    }

    public override void Exit()
    {
        base.Exit();
        Sliding = false;
        core.Movement.Flip(-core.Movement.FaceDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (timer > 0) timer -= Time.deltaTime;
        else Sliding = true;

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
