using TMPro.EditorUtilities;
using UnityEditor;

namespace Core.Client.UI
{
    [CustomEditor(typeof(ExtendedText))]
    public class ExtendedTextEditor : TMP_EditorPanelUI
    {
        public override void OnInspectorGUI()
        {
            ExtendedText text = (ExtendedText)target;
            text.IsUsingLocalization = EditorGUILayout.Toggle("Use localization", text.IsUsingLocalization);
            base.OnInspectorGUI();
        }
    }
}