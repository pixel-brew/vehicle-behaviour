namespace Core
{
    public static class Angle
    {
        public static float Normalize(float angle, float minValue = -180f)
        {
            float maxValue = minValue + 360f;
            if (angle >= minValue && angle < maxValue)
            {
                return angle;
            }

            if (angle < minValue)
            {
                angle += 360f * (float) ((int) (minValue - angle) / 360 + 1);
                return angle;
            }

            angle -= 360f * (float) ((int) (angle - maxValue) / 360 + 1);
            return angle;
        }
    }
}