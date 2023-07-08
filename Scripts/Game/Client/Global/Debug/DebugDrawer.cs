using System.Collections.Generic;
using UnityEngine;

namespace Game.Client
{
    public static class DebugDrawer
    {
        private static readonly Vector3 _offset = new Vector3(0f, 0.2f, 0f);

        private static readonly List<IDebugDrawer> _drawers = new List<IDebugDrawer>();

        static DebugDrawer()
        {
            _drawers.Add(new GameViewDebugDrawer());
            _drawers.Add(SceneViewDebugDrawer.Instance);
        }

        public static void DrawLine3d(Vector3 from, Vector3 to, Color color, float duration)
        {
            foreach (var d in _drawers)
            {
                d.DrawLine3d(from + _offset, to + _offset, color, duration);
            }
        }

        public static void DrawText3d(Vector3 position, Color color, string label, float duration)
        {
            foreach (var d in _drawers)
            {
                d.DrawText3d(position + _offset, color, label, duration);
            }
        }
        
        private static Color GetColor(float value)
        {
            if (value < 0.01f)
            {
                return Color.grey;
            }
            if (value <= 0.5f)
            {
                return Color.green;
            }
            if (value <= 0.7f)
            {
                return Color.yellow;
            }

            return Color.red;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="label"></param>
        /// <param name="value"> 0f..1f</param>
        /// <param name="duration"></param>
        public static void DrawProgressBar3d(Vector3 position, string label, float value, float duration)
        {
            foreach (var d in _drawers)
            {
                d.DrawText3d(position + _offset, GetColor(value), label, duration);
            }
        }


        public static void DrawLine2d(Vector2 from, Vector2 to, Color color, float duration)
        {
            foreach (var d in _drawers)
            {
                d.DrawLine3d(new Vector3(from.x, 0f, from.y), new Vector3(to.x, 0f, to.y), color, duration);
            }
        }

        public static void DrawText2d(Vector2 position, Color color, string label, float duration)
        {
            foreach (var d in _drawers)
            {
                d.DrawText3d(new Vector3(position.x, 0f, position.y), color, label, duration);
            }
        }
        
        
    }
}