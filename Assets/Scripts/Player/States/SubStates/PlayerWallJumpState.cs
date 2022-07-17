
using Base.FSM;
using UnityEngine;

public class PlayerWallJumpState: PlayerAbilityState
{
    public PlayerWallJumpState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.DecreaseNumOfJump();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetVelocityX(player.transform.right.x * 5f);
        core.Movement.SetVelocityY(data.jumpVelocity);
        isAbilityDone = true;
    }
}
