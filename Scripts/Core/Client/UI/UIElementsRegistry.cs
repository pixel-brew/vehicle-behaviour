
using System.Collections.Generic;
using UnityEngine;

namespace Core.Client.UI
{
    [AssetSettings("registry_ui_elements", "Resources/Registries")]
    public class UIElementsRegistry: AssetSettings<UIElementsRegistry>
    {
        [SerializeField] private List<Window> _windows = new List<Window>();
        [SerializeField] private List<Popup> _popups = new List<Popup>();
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/Registries/UI Elements")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
#endif
        public UIElement Get<T>() where T : UIElement
        {
            var targetType = typeof(T);
            foreach (var window in _windows)
            {
                if (window.GetType() == targetType)
                {
                    return window;
                }
            }
            
            foreach (var popup in _popups)
            {
                if (popup.GetType() == targetType)
                {
                    return popup;
                }
            }
            return null;
        }
        
    }
}