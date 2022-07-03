using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameInput))]
public class PlayerController : MonoBehaviour
{
    private GameInput _input;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private Collider2D _coll;

    [Header("Movement")]
    public float speed = 5.0f;

    [Header("Jump")]
    public float jumpHeight;

    [Header("Ground Check")] 
    public bool grounded;
    public Vector2 checkOffset;
    public float checkSize;
    public LayerMask checkLayer;
    
    // Animator Parameters Hash
    private static readonly int Move = Animator.StringToHash("move");
    private static readonly int Grounded = Animator.StringToHash("grounded");
    private static readonly int Jump = Animator.StringToHash("jump");
    
    private void Awake()
    {
        _input = GetComponent<GameInput>();
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _input.JumpStartEvent += OnJumpStart;
    }

    private void Update()
    {
        AnimationUpdate();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_input.moveInput == 0.0f)
        {
            _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            return;
        }
        
        // 改变人物朝向
        _spriteRenderer.flipX = _input.moveInput < 0 ? true : false;

        _rb.velocity = new Vector2(_input.moveInput * speed, _rb.velocity.y);
    }

    private void GroundCheck()
    {
        var bounds = _coll.bounds;
        var rayCastHit = Physics2D.BoxCast(bounds.center,
                                                bounds.size,
                                                0.0f,
                                                Vector2.down,
                                                checkOffset.y,
                                                checkLayer.value);
        
        grounded = rayCastHit.collider? true : false;
    }

    private void OnJumpStart()
    {
        if (!grounded) return;
        
        _rb.AddForce(jumpHeight * Vector2.up, ForceMode2D.Impulse);
        _anim.SetTrigger(Jump);
    }

    private void AnimationUpdate()
    {
        _anim.SetFloat(Move, Mathf.Abs(_input.moveInput));
        _anim.SetBool(Grounded, grounded);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = grounded? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position - (Vector3)checkOffset, Vector3.one * checkSize);
    }
#endif
}
