using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client
{
    public interface IDebugDrawer
    {
        void DrawLine3d(Vector3 from, Vector3 to, Color color, float duration);
        void DrawText3d(Vector3 position, Color color, string label, float duration);
    }

}