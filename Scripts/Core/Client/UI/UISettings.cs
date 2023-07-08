using UnityEngine;

namespace Core.Client.UI
{
    [AssetSettings("settings_ui", "Resources/Settings")]
    public class UISettings : AssetSettings<UISettings>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/UI Settings")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
#endif

        [SerializeField] float _matchWidthOrHeightFactor = 1.0f;
        [SerializeField] Vector2 _referenceResolution = new Vector2(1920, 1080);
        [SerializeField] private string _canvasName = "canvas_ui";
        [SerializeField] private string _eventSystemName = "event_system_ui";

        public float MatchWidthOrHeightFactor => _matchWidthOrHeightFactor;
        public Vector2 ReferenceResolution => _referenceResolution;
        public string CanvasName => _canvasName;
        public string EventSystemName => _eventSystemName;
    }
}