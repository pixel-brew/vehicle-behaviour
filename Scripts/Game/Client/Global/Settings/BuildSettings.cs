using Core.Client;
using UnityEngine;

namespace Game.Client
{
    [AssetSettings("settings_build", "Resources/Settings")]
    public class BuildSettings : AssetSettings<BuildSettings>
    {
        [SerializeField] private bool _showFpsInfo = true;
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/Build Settings")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
#endif

        public bool ShowFpsInfo => _showFpsInfo;
    }
}