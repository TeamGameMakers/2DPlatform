using Base.FSM;
using UnityEngine;

public class PlayerWallSlideState : PlayerAbilityState
{
    public PlayerWallSlideState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetFriction(0);
        core.Movement.SetVelocityX(player.transform.right.x);
        core.Movement.SetVelocityY(-data.wallSlideVelocity);
    }
}
