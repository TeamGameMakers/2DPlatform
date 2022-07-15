using UnityEngine;

namespace Legacy
{
    [RequireComponent(typeof(GameInput))]
    public class PlayerController : MonoBehaviour
    {
        private GameInput _input;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private Animator _anim;

        [Header("Movement")]
        [SerializeField] private float speed = 2.8f;
        // [SerializeField] private float airSpeedMultiplier = 0.5f;
        private float _currentSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 5;
        // [SerializeField] private float jumpMultiplier = 1.0f;
        // [SerializeField] private float fallMultiplier = 1.0f;
        private bool _jumping;
        private bool _canJump;

        [Header("Ground Check")] 
        [SerializeField] private bool grounded;
        [SerializeField] private Transform checkPos;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask checkLayer;

        [Header("Slope Climb")] 
        [SerializeField] private float maxAngle;
        [SerializeField] private float rayLength;
        [SerializeField] private PhysicsMaterial2D noneFriction;
        [SerializeField] private PhysicsMaterial2D fullFriction;
        private float _slopeAngle;
        private bool _onSlope;
        private bool _canMoveOnSlope;
        private Vector2 _slopeDirection;

        // Animator Parameters Hash
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Grounded = Animator.StringToHash("grounded");
        private static readonly int Jump = Animator.StringToHash("jumping");
    
        private void Awake()
        {
            _input = GetComponent<GameInput>();
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
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
            GroundCheck();
            SlopeCheck();
            Movement();
        }

        private void Movement()
        {
            var xInput = _input.moveInput;
        
            // 转向
            if (xInput != 0.0f) _spriteRenderer.flipX = xInput < 0;

            if (grounded && !_onSlope &&  !_jumping)
            {
                xInput *= _currentSpeed;
                _rb.velocity = Vector2.right * xInput;
            }
            else if (grounded && _onSlope && _canMoveOnSlope && !_jumping)
            {
                xInput *= _currentSpeed;
                _rb.velocity = _slopeDirection * -xInput;
            }
            else if (!grounded)
            {
                xInput *= _currentSpeed;
                _rb.velocity = new Vector2(xInput, _rb.velocity.y);
            }

        }

        private void GroundCheck()
        {
            grounded = Physics2D.OverlapCircle(checkPos.position, checkRadius, checkLayer.value);

            if (_rb.velocity.y < 0.0f) _jumping = false;

            if (grounded && !_jumping && _canMoveOnSlope)
                _canJump = true;
            else
                _canJump = false;
        }
    
        private void SlopeCheck()
        {
            var hit = Physics2D.Raycast(checkPos.position, Vector2.down, rayLength, checkLayer);

            // 斜坡法向量和垂直向量的夹角
            _slopeAngle = Vector2.Angle(Vector2.up, hit.normal);

            _onSlope = _slopeAngle > 0.0f;
            _canMoveOnSlope = _slopeAngle <= maxAngle;

            if (_onSlope && _canMoveOnSlope && _input.moveInput == 0.0f)
                _rb.sharedMaterial = fullFriction;
            else if (!_canMoveOnSlope)
                _rb.sharedMaterial = noneFriction;
            else
                _rb.sharedMaterial = null;
        
            // 斜坡法向量的正交向量
            _slopeDirection = Vector2.Perpendicular(hit.normal).normalized;
        
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, _slopeDirection, Color.red);
        }

        private void OnJumpStart()
        {
            if (!_canJump) return;
            _canJump = false;
            _jumping = true;    
            _rb.velocity = new Vector2(0.0f, jumpVelocity);
            // _anim.SetTrigger(Jump);
        }

        private void AnimationUpdate()
        {
            _anim.SetFloat(Run, Mathf.Abs(_input.moveInput));
            _anim.SetBool(Grounded, grounded);
            _anim.SetBool(Jump, _jumping);
        }
    
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = grounded? Color.green : Color.red;
            Gizmos.DrawWireSphere(checkPos.position, checkRadius);
        }
#endif
    }
}
