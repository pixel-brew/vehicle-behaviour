using System.Runtime.CompilerServices;

namespace Core
{
    public static class MathOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(float value)
        {
            if (value < 0.0f)
            {
                return 0.0f;
            }
            if (value > 1.0f)
            {
                return 1f;
            }
            return value;
        }
    }
}