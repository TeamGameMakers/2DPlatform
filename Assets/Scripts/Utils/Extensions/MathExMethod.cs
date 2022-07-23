using UnityEngine;

namespace Utils.Extensions
{
    public static class MathExMethod
    {
        public static Vector2 AngleToVec2(this float angle)
        {
            angle *= Mathf.Deg2Rad;
            
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            return new Vector2(x/x, y/x);
        }
    }
}
