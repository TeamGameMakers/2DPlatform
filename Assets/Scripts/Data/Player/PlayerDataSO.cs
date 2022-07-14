using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Data", fileName = "New Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Movement")] 
    public float moveVelocity = 5.0f;

    [Header("Jump")] 
    public float jumpVelocity;
}