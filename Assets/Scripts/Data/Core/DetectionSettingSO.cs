using UnityEngine;

[CreateAssetMenu(menuName = "/Data/Detection Setting", fileName = "Detection Setting")]
public class DetectionSettingSO : ScriptableObject
{
    [Header("Ground Check Settings")]
    public Vector2 groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    
    [Header("Wall Check Settings")]
    public Vector2 wallCheckPosUp;
    public Vector2 wallCheckPosMid;
    public float wallCheckLength;

    [Header("Slope Check Settings")]
    public Vector2 slopeCheckPos;
    [Tooltip("斜坡检测射线长度")]
    public float rayCastLength;
    public float maxAngle;
}
