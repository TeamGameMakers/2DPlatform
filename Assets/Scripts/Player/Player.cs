using System;
using Base.FSM;
using UnityEngine;
using Legacy;
using Core;

public class Player: Entity
{
    #region Component
    
    public Animator Anim { get; private set; }
    public GameCore Core { get; private set; }
    public GameInputHandler InputHandler { get; private set; }

    #endregion
    
    #region State Machine
    
    public StateMachine StateMachine { get; private set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerInAirState AirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    
    #endregion

    [SerializeField] private PlayerDataSO data;

    private readonly int _animVelocityY = Animator.StringToHash("velocityY");

    private void Awake()
    {
        Core = GetComponentInChildren<GameCore>();
        
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, data, "idle", StateMachine);
        MoveState = new PlayerMoveState(this, data, "move", StateMachine);
        JumpState = new PlayerJumpState(this, data, "jump", StateMachine);
        WallJumpState = new PlayerWallJumpState(this, data, "jump", StateMachine);
        AirState = new PlayerInAirState(this, data, "air", StateMachine);
        WallSlideState = new PlayerWallSlideState(this, data, "wall-slide", StateMachine);
        
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<GameInputHandler>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }
    
    private void FixedUpdate()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.PhysicsUpdate();
    }
    
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        Anim.SetFloat(_animVelocityY, Core.Movement.CurrentVelocity.y);
    }
}