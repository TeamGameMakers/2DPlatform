using System;
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
    [SerializeField] private float speed = 2.8f;
    [SerializeField] private float speedMultiplier = 0.5f;
    private float _speed;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 5;
    [SerializeField] private float jumpMultiplier = 1.0f;
    [SerializeField] private float fallMultiplier = 1.0f;
    private bool _jumping;

    [Header("Ground Check")] 
    [SerializeField] private bool grounded;
    [SerializeField] private Transform checkPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask checkLayer;

    [Header("Slope Climb")] 
    [SerializeField] private float maxAngle;
    [SerializeField] private float rayLength;
    [SerializeField] private bool onSlope;
    [SerializeField] private PhysicsMaterial2D noneFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    public float gravityScale;
    public Vector2 velocity;
        
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
        _speed = speed;
    }

    private void Update()
    {
        AnimationUpdate();
        GroundCheck();
        gravityScale = _rb.gravityScale;
        velocity = _rb.velocity;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        var xInput = _input.moveInput;
        
        if (xInput != 0.0f) _spriteRenderer.flipX = xInput < 0;

        if (xInput * _rb.velocity.x <= 0) _speed = grounded? speed : speed * speedMultiplier;
        
        if (!grounded)
        {
            _rb.gravityScale = _jumping? jumpMultiplier : fallMultiplier;
        }
        else
        {
            _rb.gravityScale = fallMultiplier;
            _speed = speed;
        }

        var dir = SlopeCheck();
        _rb.sharedMaterial = xInput == 0 ? fullFriction : noneFriction;
        if (dir != Vector2.zero && !_jumping && grounded)
        {
            _rb.velocity = new Vector2(-dir.x * xInput * _speed, -dir.y * xInput * speed);
        }
        else 
            _rb.velocity = new Vector2(xInput * _speed, _rb.velocity.y);

    }

    private void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(checkPos.position, checkRadius, checkLayer.value);

        if (_rb.velocity.y < 0) _jumping = false;
    }

    private Vector2 SlopeCheck()
    {
        var hit = Physics2D.Raycast(checkPos.position, Vector2.down, rayLength, checkLayer);

        // 斜坡法向量和垂直向量的夹角
        var angle = Vector2.Angle(Vector2.up, hit.normal);
        
        // 斜坡法向量的正交向量
        var dir = Vector2.Perpendicular(hit.normal).normalized;
        
        Debug.DrawRay(hit.point, hit.normal, Color.green);
        Debug.DrawRay(hit.point, -dir, Color.red);

        if (angle <= maxAngle && angle > 0)
        {
            onSlope = true;
            return dir;
        }
        
        onSlope = false;

        return Vector2.zero;
    }

    private void OnJumpStart()
    {
        if (!grounded) return;
        _jumping = true;
        _rb.velocity = new Vector2(_input.moveInput * _speed, jumpVelocity);
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
        Gizmos.DrawWireSphere(checkPos.position, checkRadius);
    }
#endif
}
