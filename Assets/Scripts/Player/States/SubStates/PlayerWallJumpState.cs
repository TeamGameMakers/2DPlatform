using Base.FSM;
using Codice.Client.BaseCommands.Merge.Restorer.Finder;
using Utils.Extensions;
using UnityEngine;

public class PlayerWallJumpState: PlayerAbilityState
{
    private float _time;
    private Vector2 _direction;

    public PlayerWallJumpState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.DecreaseNumOfJump();

        _direction = data.wallJumpAngle.AngleToVec2();
        _direction.Set(_direction.x * core.Movement.FaceDirection, _direction.y);
        core.Movement.SetVelocity(_direction * data.wallJumpVelocity);
        _time = data.wallJumpTime;
        player.InputHandler.LockMoveInputX(core.Movement.FaceDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        _time -= Time.deltaTime;
        if (_time < 0 || core.Detection.touchWall || core.Detection.touchLedge)
        {
            isAbilityDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.InputHandler.UnLockMoveInputX();
    }
}
