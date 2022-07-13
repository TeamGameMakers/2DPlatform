using Core.FSM;

public class PlayerGroundedState: PlayerState
{
    protected float xInput;
    public PlayerGroundedState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) : 
        base(player, data, animBoolName, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}