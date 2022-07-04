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
    public float speed = 2.8f;
    public float speedMultiplier = 0.5f;
    private float _speed;

    [Header("Jump")]
    public float jumpVelocity = 5;
    public float jumpMultiplier = 1.0f;
    public float fallMultiplier = 1.0f;

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
        
        // 当在空中移动方向改变时，减慢移动速度
        if (_rb.velocity.x * _input.moveInput <= 0)
            _speed = grounded ? speed : speed * speedMultiplier;
        
        // 改变人物朝向
        _spriteRenderer.flipX = _input.moveInput < 0;
        
        _rb.velocity = new Vector2(_input.moveInput * _speed, _rb.velocity.y);
    }

    private void GroundCheck()
    {
        var bounds = _coll.bounds;
        var rayCastHit = Physics2D.BoxCast(bounds.center,
                                                Vector2.one * checkSize,
                                                0.0f,
                                                Vector2.down,
                                                checkOffset.y,
                                                checkLayer.value);

        grounded = rayCastHit.collider;
        
        if (_rb.velocity.y < 0 && !grounded)
            _rb.gravityScale = fallMultiplier;
        else
            _rb.gravityScale = jumpMultiplier;
    }

    private void OnJumpStart()
    {
        if (!grounded) return;
        
        _rb.velocity = new Vector2(_rb.velocity.x, jumpVelocity);
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
