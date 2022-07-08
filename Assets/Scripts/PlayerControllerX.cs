using UnityEngine;

[RequireComponent(typeof(GameInput))]
public class PlayerControllerX : MonoBehaviour
{
    private GameInput _input;
    private Rigidbody2D _rb;
    private Animator _anim;
    private CollisionDetector _collDetector;
    
    [Header("Movement")]
    [SerializeField] private float speed = 3.5f;
    // [SerializeField] private float airSpeedMultiplier = 0.5f;
    private float _currentSpeed;
    
    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 5;
    // [SerializeField] private float jumpMultiplier = 1.0f;
    // [SerializeField] private float fallMultiplier = 1.0f;
    private bool _jumping;
    private bool _canJump;
    
    // Animator Parameters Hash
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Grounded = Animator.StringToHash("grounded");
    private static readonly int Jump = Animator.StringToHash("jumping");
    private static readonly int Wall = Animator.StringToHash("wall");

    private void Awake()
    {
        _input = GetComponent<GameInput>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _collDetector = GetComponent<CollisionDetector>();
    }

    private void Start()
    {
        _input.JumpStartEvent += OnJumpStart;
        _currentSpeed = speed;
    }

    private void Update()
    {
        AnimationUpdate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var xInput = _input.moveInput * _currentSpeed;

        Flip();

        if (_rb.velocity.y < 0) _jumping = false;

        switch (_collDetector.location)
        {
            case CollisionDetector.PlayerLocation.Platform:
                if (!_jumping)
                {
                    _rb.velocity = Vector2.right * xInput;
                    _canJump = true;
                }
                break;
            
            case CollisionDetector.PlayerLocation.Slope:
                if (!_jumping)
                {
                    _rb.velocity = _collDetector.SlopeDirection * xInput;
                    _canJump = true;
                }
                break;
            
            case CollisionDetector.PlayerLocation.Steep:
                _canJump = false;
                break;
            
            case CollisionDetector.PlayerLocation.Air:
                _canJump = false;
                _rb.velocity = new Vector2(xInput, _rb.velocity.y);
                break;
            
            case CollisionDetector.PlayerLocation.Wall:
                _rb.velocity = new Vector2(xInput, -3f);
                _jumping = false;
                break;
            
            case CollisionDetector.PlayerLocation.Edge:
                break;
        }
    }

    private void Flip()
    {
        var scale = transform.localScale;
        
        if (scale.x * _input.moveInput < 0) scale.Set(-scale.x, scale.y,scale.z);

        transform.localScale = scale;
    }

    private void OnJumpStart()
    {
        if (!_canJump) return;
        _canJump = false;
        _jumping = true;
        _rb.velocity = new Vector2(0.0f, jumpVelocity);
    }
    
    private void AnimationUpdate()
    {
        _anim.SetFloat(Run, Mathf.Abs(_input.moveInput));
        _anim.SetBool(Grounded, _collDetector.onGround);
        _anim.SetBool(Jump, _jumping);
        _anim.SetBool(Wall, _collDetector.onWall);
    }
}
