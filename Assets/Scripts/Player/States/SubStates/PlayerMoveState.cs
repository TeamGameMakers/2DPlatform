using Core.FSM;
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

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
        player.SetVelocityX(xInput * data.moveVelocity);
        player.Flip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}