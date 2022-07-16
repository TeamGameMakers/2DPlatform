using Base.FSM;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
        
        core.Movement.SetVelocityX(0.0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        core.Movement.SetFriction(player.CollDetector.onSlope ? 100000 : 0.4f);

        if (InputX != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
