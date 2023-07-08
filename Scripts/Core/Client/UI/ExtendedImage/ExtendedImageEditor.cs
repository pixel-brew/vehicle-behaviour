using UnityEditor;

namespace Core.Client.UI
{
    [CustomEditor(typeof(ExtendedImage))]
    public class ExtendedImageEditor : UnityEditor.UI.ImageEditor
    {
        public override void OnInspectorGUI()
        {
            ExtendedImage image = (ExtendedImage)target;
            image.IsUsingSpriteAtlas = EditorGUILayout.Toggle("Use sprite atlas", image.IsUsingSpriteAtlas);
            base.OnInspectorGUI();
            
            //EditorGUILayout.HelpBox($" {nameof(bottomWaterLineValue)} should be less than {nameof(topWaterLineValue)} ", MessageType.Error);
        }
    }
}