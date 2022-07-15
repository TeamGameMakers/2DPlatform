using System;
using Base.FSM;
using UnityEngine;
using Legacy;
using Core;

public class Player: Entity
{
    #region Component
    
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public GameCore Core { get; private set; }
    public GameInputHandler InputHandler { get; private set; }
    public CollisionDetector CollDetector { get; private set; }
    
    #endregion
    
    #region State Machine
    
    public StateMachine StateMachine { get; private set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState AirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    
    #endregion

    private Vector2 _currentVelocity;
    
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity => _currentVelocity;

    [SerializeField] private PlayerDataSO data;

    private Vector2 _vec2Setter;

    private readonly int _animVelocityY = Animator.StringToHash("velocityY");

    private void Awake()
    {
        Core = GetComponentInChildren<GameCore>();
        
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, data, "idle", StateMachine);
        MoveState = new PlayerMoveState(this, data, "move", StateMachine);
        JumpState = new PlayerJumpState(this, data, "jump", StateMachine);
        AirState = new PlayerInAirState(this, data, "air", StateMachine);
        WallSlideState = new PlayerWallSlideState(this, data, "wall-slide", StateMachine);
        
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<GameInputHandler>();

        CollDetector = GetComponent<CollisionDetector>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }
    
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    
    private void Update()
    {
        _currentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
        Anim.SetFloat(_animVelocityY, CurrentVelocity.y);
    }

    #region Value Setter
    
    public void SetVelocity(Vector2 velocity)
    {
        Rb.velocity = velocity;
        _currentVelocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        _vec2Setter.Set(velocityX, _currentVelocity.y);
        Rb.velocity = _vec2Setter;
        _currentVelocity = _vec2Setter;
    }

    public void SetVelocityY(float velocityY)
    {
        _vec2Setter.Set(_currentVelocity.x, velocityY);
        Rb.velocity = _vec2Setter;
        _currentVelocity = _vec2Setter;
    }

    public void SetFriction(float friction)
    {
        var material = Rb.sharedMaterial;
        material.friction = friction;
        Rb.sharedMaterial = material;
    }
    
    #endregion

    public void Flip()
    {
        if (FacingDirection * InputHandler.NormInputX >= 0) return;
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}