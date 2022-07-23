using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
namespace Legacy
{
    [InitializeOnLoad]
#endif
    public class StickAxisProcessor : InputProcessor<float>
    {
#if UNITY_EDITOR
        static StickAxisProcessor()
        {
            Initialize();
        }
#endif
    
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            InputSystem.RegisterProcessor<StickAxisProcessor>();
        }
    
    
        public override float Process(float value, InputControl control)
        {
            return value switch
            {
                > 0 => 1.0f,
                0 => 0.0f,
                _ => -1.0f
            };
        }
    }
}
