using UnityEngine;

[ExecuteInEditMode]
public class CollisionDetector : MonoBehaviour
{
    private GameInput _input;
    private Collider2D _coll;
    
    public enum PlayerLocation
    {
        Platform,
        Slope,
        Steep,
        Air,
        Wall,
        Edge
    }
    public PlayerLocation location;

    [Header("Ground Check Settings")]
    [SerializeField] private Vector2 groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    
    [Header("Wall Check Settings")]
    [SerializeField] private Vector2 wallCheckPos;
    [SerializeField] private float wallCheckLength;
    
    [Header("Slope Check Settings")]
    [SerializeField] private Vector2 slopeCheckPos;
    [Tooltip("斜坡检测射线长度")]
    [SerializeField] private float rayCastLength;
    [SerializeField] private float maxAngle;
    private float _slopeAngle;
    private Vector2 _slopeDirection;
    public Vector2 SlopeDirection => -_slopeDirection;

    [Header("Check")]
    public bool onGround;
    public bool onSlope;
    public bool onWall;
    public bool touchWall;

    private LayerMask _ground;
    private LayerMask _wall;
    private Vector2 _faceDirection = Vector2.right;

    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
        _input = GetComponent<GameInput>();
        _ground = LayerMask.GetMask("Ground", "Wall");
        _wall = LayerMask.GetMask("Wall");
    }

    private void FixedUpdate()
    {
        Check();
        LocationUpdate();
    }

    private void Check()
    {
        if (transform.localScale.x * _faceDirection.x < 0)
            _faceDirection *= -1;
        
        // 地面检测
        Vector2 pos = _coll.bounds.center;
        onGround = Physics2D.OverlapCircle(pos + groundCheckPos, groundCheckRadius, _ground);
            
        // 墙面检测
        bool wallCheck = Physics2D.Raycast(pos + wallCheckPos, _faceDirection, wallCheckLength, _wall);
        if (location == PlayerLocation.Air)
        {
            if (wallCheck && _input.moveInput != 0) onWall = true;
        }
        else if (_input.moveInput * transform.localScale.x > 0 || onGround)
        {
            onWall = false;
        }
        touchWall = wallCheck;

        // 坡度检测
        var hit = Physics2D.Raycast(pos + slopeCheckPos, Vector2.down, rayCastLength, _ground);
        _slopeAngle = Vector2.Angle(Vector2.up, hit.normal);
        _slopeDirection = Vector2.Perpendicular(hit.normal);
        
        Debug.DrawRay(hit.point, hit.normal, Color.green);
        Debug.DrawRay(hit.point, _slopeDirection, Color.red);
        
    }

    private void LocationUpdate()
    {
        if (onGround && _slopeAngle == 0)
            location = PlayerLocation.Platform;

        if (onGround && _slopeAngle > 0 && _slopeAngle <= maxAngle)
            location = PlayerLocation.Slope;

        if (onGround && _slopeAngle > maxAngle)
            location = PlayerLocation.Steep;

        if (!onGround && !onWall)
            location = PlayerLocation.Air;
        
        if (!onGround && onWall) 
            location = PlayerLocation.Wall;
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        var pos = _coll.bounds.center;

        Gizmos.color = onGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(pos + (Vector3)groundCheckPos, groundCheckRadius);
        
        var from = pos + (Vector3)wallCheckPos;
        Gizmos.color = onWall ? Color.green : Color.red;
        Gizmos.DrawLine(from, from + (Vector3)_faceDirection * wallCheckLength);

        from = pos + (Vector3)slopeCheckPos;
        Gizmos.color = onSlope ? Color.green : Color.red;
        Gizmos.DrawLine(from, from + Vector3.down * rayCastLength);
    }
    #endif
}
