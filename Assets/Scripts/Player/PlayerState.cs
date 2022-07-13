using Core.FSM;
using UnityEngine;

public class PlayerState : State
{
    protected readonly Player player;
    protected PlayerDataSO data;
    private readonly int _animBoolHash;

    public PlayerState(Player player, PlayerDataSO data, string animBoolName,StateMachine stateMachine) : 
        base(stateMachine)
    {
        this.player = player;
        this.data = data;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public override void Enter()
    {
        player.Anim.SetBool(_animBoolHash, true);
    }

    public override void Exit()
    {
        player.Anim.SetBool(_animBoolHash, false);
    }
}