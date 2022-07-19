using Base.FSM;
using Core;
using UnityEngine;

public class PlayerState : State
{
    protected readonly GameCore core;
    protected readonly Player player;
    protected readonly PlayerDataSO data;
    protected float startTime;
    
    private readonly int _animBoolHash;
    
    protected bool Exiting { get; private set; }
    protected JumpInputInfo JumpInput { get; private set; }
    protected int InputX { get; private set; }
    protected int InputY { get; private set; }

    protected PlayerState(Player player, PlayerDataSO data, string animBoolName,StateMachine stateMachine) : 
        base(stateMachine)
    {
        this.player = player;
        this.data = data;
        core = player.Core;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public override void Enter()
    {
        startTime = Time.time;
        player.Anim.SetBool(_animBoolHash, true);
        JumpInput = player.InputHandler.JumpInput;
        InputX = player.InputHandler.NormInputX;
        InputY = player.InputHandler.NormInputY;
        Exiting = false;
    }

    public override void LogicUpdate()
    {
        JumpInput = player.InputHandler.JumpInput;
        InputX = player.InputHandler.NormInputX;
        InputY = player.InputHandler.NormInputY;
    }

    public override void Exit()
    {
        player.Anim.SetBool(_animBoolHash, false);
        Exiting = true;
    }
}