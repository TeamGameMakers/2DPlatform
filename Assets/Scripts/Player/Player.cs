using System;
using UnityEngine;
using Core.FSM;
using Legacy;

public class Player: Entity
{
    #region State Machine
    
    public StateMachine StateMachine { get; private set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState AirState { get; private set; }
    
    #endregion
    
    #region Component
    
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public GameInputHandler InputHandler { get; private set; }
    
    public CollisionDetector CollDetector { get; private set; }
    
    #endregion

    private Vector2 _currentVelocity;
    
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity => _currentVelocity;

    [SerializeField] private PlayerDataSO data;

    private Vector2 _vec2Setter;

    private void Awake()
    {
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, data, "idle", StateMachine);
        MoveState = new PlayerMoveState(this, data, "move", StateMachine);
        JumpState = new PlayerJumpState(this, data, "air", StateMachine);
        AirState = new PlayerInAirState(this, data, "air", StateMachine);
        
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<GameInputHandler>();

        CollDetector = GetComponent<CollisionDetector>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }

    private void Update()
    {
        _currentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocityX)
    {
        _vec2Setter.Set(velocityX, _currentVelocity.y);
        RB.velocity = _vec2Setter;
        _currentVelocity = _vec2Setter;
    }

    public void SetVelocityY(float velocityY)
    {
        _vec2Setter.Set(_currentVelocity.x, velocityY);
        RB.velocity = _vec2Setter;
        _currentVelocity = _vec2Setter;
    }

    public void Flip()
    {
        if (FacingDirection * InputHandler.NormInputX >= 0) return;
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}