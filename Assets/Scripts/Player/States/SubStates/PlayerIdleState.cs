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

        if (InputX != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        
        if (core.Detection.onSlope)
            core.Movement.SetFriction(100000);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
