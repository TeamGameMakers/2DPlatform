using Base.FSM;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _numOfJump;

    public bool CanJump => _numOfJump > 0;

    public PlayerJumpState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) :
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _numOfJump--;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.SetVelocityY(data.jumpVelocity);
        player.AirState.StartJumping();
        isAbilityDone = true;
    }

    public void ResetNumOfJump() => _numOfJump = data.numOfJump;
    public void DecreaseNumOfJump() => _numOfJump--;
}
