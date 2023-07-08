using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

namespace Core.Client.UI
{
    [AssetSettings("settings_sprite_atlases", "Resources/Settings")]
    public class SpriteAtlasSettings : AssetSettings<SpriteAtlasSettings>
    {
        [Serializable]
        class SpriteAtlasesPerScene
        {
            [SerializeField][Scene] public string SceneName;
            [SerializeField] public List<SpriteAtlas> ValidAtlases;
        }
    
        [SerializeField] private List<SpriteAtlasesPerScene> _validAtlasesPerScene;
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/Sprite Atlases Settings")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
#endif

        public List<SpriteAtlas> GetValidAtlases(string sceneName)
        {
            foreach (var atlasesPerScene in _validAtlasesPerScene)
            {
                if (atlasesPerScene.SceneName == sceneName)
                {
                    return atlasesPerScene.ValidAtlases;
                }
            }
            Debug.LogError("sprite atlas validation :: cannot find scene with such name " + sceneName);
            return null;
        }
    }
}