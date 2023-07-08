using System;
namespace Core
{
    public static class FloatExtensions
    {
        public static bool IsZero(this float target)
        {
            return Math.Abs(target) <= float.Epsilon;
        }
        
        public static float Threshold(this float target, float value)
        {
            return Math.Abs(target) <= value ? 0f : target;
        }
        
        public static float Clamp01(this float target)
        {
            if (target < 0f)
            {
                return 0f;
            }

            if (target > 1f)
            {
                return 1f;
            }

            return target;
        }
        
        public static bool IsApproximatelyEqual(this float me, float target)
        {
            return Math.Abs(target - me) <= float.Epsilon;
        }
    }
}