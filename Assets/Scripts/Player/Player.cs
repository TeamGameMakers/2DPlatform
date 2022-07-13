using System;
using UnityEngine;
using Core.FSM;

public class Player: Entity
{
    #region State Machine
    
    public StateMachine StateMachine { get; private set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    
    #endregion
    
    #region Component
    
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public GameInputHandler InputHandler { get; private set; }
    
    #endregion
    
    public int FacingDirection { get; private set; }

    [SerializeField] private PlayerDataSO data;

    private void Awake()
    {
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, data, "idle", StateMachine);
        MoveState = new PlayerMoveState(this, data, "move", StateMachine);
        
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<GameInputHandler>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocity)
    {
        RB.velocity = new Vector2(velocity * data.speed, RB.velocity.y);
    }

    public void Flip()
    {
        if (FacingDirection * InputHandler.NormInputX >= 0) return;
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}