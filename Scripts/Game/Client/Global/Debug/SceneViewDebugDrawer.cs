using System.Collections.Generic;
using Core.Client;
using UnityEngine;

namespace Game.Client
{
    public class SceneViewDebugDrawer : MonoSingleton<SceneViewDebugDrawer>, IDebugDrawer 
    {
        private readonly GUIStyle _style = new GUIStyle();
        private readonly List<TextData> _textData = new List<TextData>();
        
        private struct TextData
        {
            public Vector2 Position;
            public string Text;
            public Color Color;
            public float Duration;
        }
        
        void IDebugDrawer.DrawLine3d(Vector3 @from, Vector3 to, Color color, float duration)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.DrawLine(from, to, color, duration);
#endif
        }

        void IDebugDrawer.DrawText3d(Vector3 position, Color color, string label, float duration)
        {
#if UNITY_EDITOR
            _textData.Add(new TextData()
            {
                Position = position,
                Text = label,
                Color = color,
                Duration = duration
            });
            #endif
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            lock (_textData)
            {
                for (var i = _textData.Count - 1; i > -1; i--)
                {
                    var label = _textData[i];

                    _style.normal.textColor = label.Color;
                    UnityEditor.Handles.Label(label.Position, label.Text, _style);
                    label.Duration -= Time.deltaTime;
                    if (label.Duration <= 0.0f)
                    {
                        _textData.RemoveAt(i);
                    }
                }
            }
        }
#endif
    }
}
