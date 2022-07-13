using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Data", fileName = "New Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Movement")] 
    public float speed = 5.0f;
}