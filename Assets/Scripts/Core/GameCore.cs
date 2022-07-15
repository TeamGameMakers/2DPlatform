using UnityEngine;

namespace Core
{
    public class GameCore : MonoBehaviour
    {
        public Movement Movement { get; private set; }

        private void Awake()
        {
            Movement = GetComponentInChildren<Movement>();
            
            if (!Movement) Debug.LogError("Missing Movement Component In Children");
        }

        private void Start()
        {
            Movement.core = this;
        }
    }
}
