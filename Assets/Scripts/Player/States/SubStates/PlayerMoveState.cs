using Base.FSM;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (InputX == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.CollDetector.onSlope)
        {
            core.Movement.SetFriction(0);
            core.Movement.SetVelocity(InputX * data.moveVelocity * player.CollDetector.SlopeDirection);
        }
        else
            core.Movement.SetVelocityX(InputX * data.moveVelocity);
        player.Flip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}