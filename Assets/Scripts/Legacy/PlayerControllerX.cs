using System.Collections;
using UnityEngine;

namespace Legacy
{
    [RequireComponent(typeof(GameInput))]
    public class PlayerControllerX : MonoBehaviour
    {
        private GameInput _input;
        private Rigidbody2D _rb;
        private Animator _anim;
        private CollisionDetector _collDetector;

        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float wallSlideSpeed = 1;
        private float _currentSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 10;
        private bool _jumping;
        private bool _canJump;

        private PhysicsMaterial2D _material;
    
        // Animator Parameters Hash
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Grounded = Animator.StringToHash("grounded");
        private static readonly int Jump = Animator.StringToHash("jumping");
        private static readonly int Wall = Animator.StringToHash("wall");
        private static readonly int Ledge = Animator.StringToHash("ledge");

        private void Awake()
        {
            _input = GetComponent<GameInput>();
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _collDetector = GetComponent<CollisionDetector>();
            _material = _rb.sharedMaterial;
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

            Flip(xInput);

            if (_rb.velocity.y < 0)
            {
                _jumping = false;
            
                // 下落时更新一次移动输入来优化贴墙跳跃手感
                _input.UpdateMoveInput();
            }

            switch (_collDetector.location)
            {
                case CollisionDetector.PlayerLocation.Platform:
                    if (!_jumping)
                    {
                        _material.friction = 0.4f;
                        _rb.velocity = Vector2.right * xInput;
                        _canJump = true;
                    }
                    break;
            
                case CollisionDetector.PlayerLocation.Slope:
                    if (!_jumping)
                    {
                        _material.friction = xInput == 0 ? 100000 : 0;
                        _rb.velocity = _collDetector.SlopeDirection * xInput;
                        _canJump = true;
                    }
                    break;
            
                case CollisionDetector.PlayerLocation.Steep:
                    _material.friction = 0;
                    _canJump = false;
                    break;
            
                case CollisionDetector.PlayerLocation.Air:
                    _material.friction = 0;
                    _canJump = false;
                    _rb.velocity = new Vector2(xInput, _rb.velocity.y);
                    break;
            
                case CollisionDetector.PlayerLocation.Wall:
                    _canJump = true;
                    Flip(-xInput);
                    _rb.velocity = new Vector2(-transform.localScale.x * 2, -wallSlideSpeed);
                    _material.friction = 0;
                    _jumping = false;
                    break;
            
                case CollisionDetector.PlayerLocation.Ledge:
                    _canJump = true;
                    if (!_jumping)
                    {
                        //BUG: 穿墙
                        _rb.velocity = new Vector2(transform.localScale.x * 2, _rb.velocity.y);
                        _material.friction = _collDetector.ledgeDistance < 0.01f ? 100000 : 0;
                    }
                    break;
            }
        
            _rb.sharedMaterial = _material;
        }

        private void Flip(float faceDirection)
        {
            var scale = transform.localScale;
        
            if (scale.x * faceDirection < 0) scale.Set(-scale.x, scale.y,scale.z);

            transform.localScale = scale;
        }

        private void OnJumpStart()
        {
            if (!_canJump) return;
            _canJump = false;
            _jumping = true;
        
            switch (_collDetector.location)
            {
                case CollisionDetector.PlayerLocation.Wall:
                    StartCoroutine(WallJump(0.5f));
                    break;
                case CollisionDetector.PlayerLocation.Ledge:
                    _material.friction = 0;
                    _rb.velocity = Vector2.up * jumpVelocity;
                    break;
                case CollisionDetector.PlayerLocation.Slope or CollisionDetector.PlayerLocation.Platform:
                    _rb.velocity = Vector2.up * jumpVelocity;
                    if (_collDetector.touchWall) _input.moveInput = 0;
                    break;
            }
        
            // if (_collDetector.location == CollisionDetector.PlayerLocation.Wall)
            //     StartCoroutine(WallJump(0.5f));
            // else
            // {
            //     _rb.velocity = Vector2.up * jumpVelocity;
            //     if (_collDetector.touchWall) _input.moveInput = 0;
            // }
        }

        private IEnumerator WallJump(float waitTime)
        {
            _input.moveInput = transform.localScale.x;
            _input.canMove = false; 
            _rb.velocity = Vector2.up * jumpVelocity;
        
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                _rb.velocity = new Vector2(_input.moveInput * speed, _rb.velocity.y);
                _rb.sharedMaterial.friction = 10;
                yield return null;
            }

            _input.moveInput = 0;
            _input.canMove = true;
            _input.UpdateMoveInput();
        }
    
    
        private void AnimationUpdate()
        {
            _anim.SetFloat(Run, Mathf.Abs(_input.moveInput));
            _anim.SetBool(Grounded, _collDetector.onGround);
            _anim.SetBool(Jump, _jumping);
            _anim.SetBool(Wall, _collDetector.onWall);
            _anim.SetBool(Ledge, _collDetector.onLedge);
        }
    }
}
