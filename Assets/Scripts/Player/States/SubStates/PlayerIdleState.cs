using Base.FSM;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
        
        if (core.Detection.onSlope)
            core.Movement.SetFriction(100000);
        else if (core.Detection.onSteep)
            core.Movement.SetFriction(0);
        else
            core.Movement.SetFriction(0.3f);
        
        core.Movement.SetVelocity(Vector2.zero);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Exiting) return;

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
