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

        if (Exiting) return;
        
        if (core.Detection.onSlope)
            core.Movement.SetFriction(100000);
        else if (core.Detection.onSteep)
            core.Movement.SetFriction(0);
        else
            core.Movement.SetFriction(0.3f);

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
