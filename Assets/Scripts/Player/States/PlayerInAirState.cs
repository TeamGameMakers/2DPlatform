using Core.FSM;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private readonly int _animVelocityY = Animator.StringToHash("velocityY");
    
    public PlayerInAirState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        player.Anim.SetFloat(_animVelocityY, player.CurrentVelocity.y);
        player.Flip();

        if (player.CollDetector.onGround)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(player.InputHandler.NormInputX * data.moveVelocity);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
