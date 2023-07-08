using UnityEngine;

namespace Game.Client
{
    public class GameViewDebugDrawer  : IDebugDrawer
    {
        public int FontSize = 20;

        void IDebugDrawer.DrawLine3d(Vector3 @from, Vector3 to, Color color, float duration)
        {
            IMDraw.Line3D(from, to, color, duration);
        }

        void IDebugDrawer.DrawText3d(Vector3 position, Color color, string label, float duration)
        {
            IMDraw.Label(position, color, FontSize, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, label, duration);
        }
    }
}