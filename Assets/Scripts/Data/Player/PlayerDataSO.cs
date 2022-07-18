using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Data", fileName = "New Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Movement")] 
    public float moveVelocity = 5.0f;

    [Header("Jump")] 
    public float jumpVelocity;
    public int numOfJump = 1;
    public float jumpHeightMultiplier;

    public float wallJumpVelocity;
    public float wallJumpTime;
    [Range(0, 90)] public float wallJumpAngle;

    [Header("Air")] 
    public float coyoteTime;

    [Header("Wall")] 
    public float wallSlideVelocity;
}