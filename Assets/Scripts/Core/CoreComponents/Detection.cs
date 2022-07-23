using UnityEngine;

namespace Core
{
    [ExecuteInEditMode]
    public class Detection : CoreComponent
    {
        private Collider2D _coll;
        private Vector2 _center;
        private LayerMask _ground;
        private LayerMask _wall;
        
        #region Settings
        
        [Header("Ground Check Settings")] 
        [SerializeField] private Vector2 groundCheckPos;
        [SerializeField] private float groundCheckRadius;

        [Header("Wall Check Settings")] 
        [SerializeField] private Vector2 wallCheckPosUp;
        [SerializeField] private Vector2 wallCheckPosMid;
        [SerializeField] private float wallCheckLength;

        [Header("Slope Check Settings")] 
        [SerializeField] private float slopeCheckDistance;
        [SerializeField] private float maxAngle;
        
        #endregion

        [Header("Check")] 
        public bool grounded;
        public bool touchWall;
        public bool touchLedge;
        public bool onSlope;
        public bool onSteep;

        private float _slopeAngle;
        public float SlopeAngle => _slopeAngle;
        public Vector2 SlopeDirection { get; private set; }
        
        public int WallLocation { get; private set; }
        
        public Vector2 LedgePosition { get; private set; }

        private void Awake()
        {
            _coll =  GetComponentInParent<Collider2D>();
            _center = _coll.bounds.center;
            _ground = LayerMask.GetMask("Ground", "Wall");
            _wall = LayerMask.GetMask("Wall");
            LedgePosition = new Vector2();
        }

        internal void LogicUpdate()
        {
            _center = _coll.bounds.center;
            grounded = Physics2D.OverlapCircle(_center + groundCheckPos, groundCheckRadius, _ground);
            GetSlopeInfo();
            GetWallInfo();
        }

        private void GetSlopeInfo()
        {
            var hit = Physics2D.Raycast( _center + groundCheckPos, Vector2.down, slopeCheckDistance, _ground);
            
            _slopeAngle = Vector2.Angle(Vector2.up, hit.normal);
            SlopeDirection = _slopeAngle <= maxAngle ? -Vector2.Perpendicular(hit.normal) : Vector2.zero;
            onSlope = _slopeAngle > 0 && _slopeAngle <= maxAngle;
            onSteep = _slopeAngle > maxAngle;

            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, SlopeDirection, Color.red);
        }

        private void GetWallInfo()
        {
            var right = transform.right;
            var mid = Physics2D.Raycast(_center + wallCheckPosMid, right, wallCheckLength, _wall);
            var up = Physics2D.Raycast(_center + wallCheckPosUp, right, wallCheckLength, _wall);

            vec2Setter.Set(wallCheckPosUp.x + mid.distance * right.x, wallCheckPosUp.y);
            var vert = Physics2D.Raycast(_center + vec2Setter, Vector2.down, wallCheckPosUp.y, _wall);
            vec2Setter.Set(_center.x + mid.distance * right.x, _center.y - vert.distance);
            LedgePosition = vec2Setter;

            touchWall = mid && up;
            touchLedge = mid && !up;

            WallLocation = touchWall ? (int)-mid.normal.x : 0;
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            // 地面检测
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_center + groundCheckPos, groundCheckRadius);
            
            // 墙面检测
            Vector3 from = _center + wallCheckPosUp;
            Gizmos.color = touchLedge ? Color.green : Color.red;
            Gizmos.DrawLine(from, from + transform.right * wallCheckLength);
            
            from = _center + wallCheckPosMid;
            Gizmos.color = touchWall ? Color.green : Color.red;
            Gizmos.DrawLine(from, from + transform.right * wallCheckLength);
            
            // 坡度检测
            from = _center + groundCheckPos;
            Gizmos.color = _slopeAngle > 0 ? Color.green : Color.red;
            Gizmos.DrawLine(from, from + Vector3.down * slopeCheckDistance);
        }
#endif
    }
}