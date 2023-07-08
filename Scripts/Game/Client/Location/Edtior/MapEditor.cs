using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Client.Location.Editor
{
    public class MapEditor : EditorWindow
    {
        private bool _isPaintMode = false;
        private Vector2 _cellSize = new Vector2(1f, 1f);
        
        [SerializeField]
        private List<GameObject> _palette = new List<GameObject>();

        private string _path = "Assets/Tiles";
        
        [SerializeField]
        private int _paletteIndex;
        
        [MenuItem("Window/Map Editor")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MapEditor));
        }
        
        private void OnGUI()
        {
            _isPaintMode = GUILayout.Toggle(_isPaintMode, "Start painting", "Button", GUILayout.Height(60f));
            
            List<GUIContent> paletteIcons = new List<GUIContent>();
            foreach (GameObject prefab in _palette)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                paletteIcons.Add(new GUIContent(texture));
            }
            
            _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), 6);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (_isPaintMode)
            {
                Vector2 cellCenter = GetSelectedCell();
                
                DisplayVisualHelp();
                HandleSceneViewInputs(cellCenter);
                
                sceneView.Repaint();
            }
        }

        private Vector2 GetSelectedCell()
        {
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);
            
            Vector2Int cell = new Vector2Int(Mathf.RoundToInt(mousePosition.x / _cellSize.x), 
                Mathf.RoundToInt(mousePosition.y / _cellSize.y));
            Vector2 cellCenter = cell * _cellSize;

            return cellCenter;
        }

        private void HandleSceneViewInputs(Vector2 cellCenter)
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }
            
            if (_paletteIndex < _palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GameObject prefab = _palette[_paletteIndex];
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                
                gameObject.transform.position = cellCenter - Vector2.right;

                // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                Undo.RegisterCreatedObjectUndo(gameObject, "");
            }
        }
        
        private void DisplayVisualHelp()
        {
            Vector2 cellCenter = GetSelectedCell();
            
            Vector3 top = cellCenter +  Vector2.up * _cellSize * 0.5f;
            Vector3 right = cellCenter - Vector2.left * _cellSize;
            Vector3 left = cellCenter + Vector2.left * _cellSize;
            Vector3 down = cellCenter - Vector2.up * _cellSize * 0.5f;
            
            Handles.color = Color.green;
            Vector3[] lines = { top, right, right, down, down, left, left, top };
            Handles.DrawLines(lines);
        }

        
        private void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
            
            RefreshPalette();
        }
        
        private void RefreshPalette()
        {
            _palette.Clear();

            string[] prefabFiles = System.IO.Directory.GetFiles(_path , "*.prefab");
            foreach (string prefabFile in prefabFiles)
            {
                _palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
            }
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
    }
}