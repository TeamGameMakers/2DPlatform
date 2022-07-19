using Base.FSM;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool _inCoyoteTime;
    private bool _jumping;

    public PlayerInAirState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        core.Movement.SetFriction(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        core.Movement.Flip(InputX);

        // 状态转换
        if (core.Detection.grounded && core.Movement.CurrentVelocity.y < 0.01f)
            stateMachine.ChangeState(player.IdleState);
        
        else if (core.Detection.grounded && core.Detection.onSlope && core.Movement.CurrentVelocity.y < 5f)
            stateMachine.ChangeState(player.IdleState);
        
        else if (core.Detection.touchWall && InputX * core.Detection.WallLocation > 0)
            stateMachine.ChangeState(player.WallSlideState);
        
        if (JumpInput.Press) 
        {
            CheckCoyoteTime();
            if (player.JumpState.CanJump)
                stateMachine.ChangeState(player.JumpState);
            else
                JumpInput.UseJumpInput();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetVelocityX(InputX * data.moveVelocity);

        if (_jumping)
        {
            if (!JumpInput.Hold)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * data.jumpHeightMultiplier);
                _jumping = false;
            }
            else if (core.Movement.CurrentVelocity.y < 0.0f)
            {
                _jumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (_inCoyoteTime && Time.time > startTime + data.coyoteTime)
        {
            player.JumpState.DecreaseNumOfJump();
            _inCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => _inCoyoteTime = true;
    public void StartJumping() => _jumping = true;
}
