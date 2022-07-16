using Base.FSM;
using UnityEngine;

public class PlayerWallSlideState : PlayerAbilityState
{
    public PlayerWallSlideState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        core.Movement.Flip(-player.transform.right.x);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (core.Detection.grounded)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetFriction(0);
        core.Movement.SetVelocityX(-player.transform.right.x * 2);
        core.Movement.SetVelocityY(-data.wallSlideVelocity);
    }
}
