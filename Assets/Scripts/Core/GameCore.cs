using UnityEngine;

namespace Core
{
    public class GameCore : MonoBehaviour
    {
        public Movement Movement { get; private set; }
        public Detection Detection { get; private set; }

        private void Awake()
        {
            Movement = GetComponentInChildren<Movement>();
            Detection = GetComponentInChildren<Detection>();
            
            if (!Movement) Debug.LogError("Missing Movement Component In Children");
            if (!Detection) Debug.LogError("Missing Detection Component In Children");
        }

        private void Start()
        {
            Movement.core = this;
        }

        public void LogicUpdate()
        {
            Detection.LogicUpdate();
            Movement.LogicUpdate();
        }
    }
}
