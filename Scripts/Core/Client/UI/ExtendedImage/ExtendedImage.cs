using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Client.UI
{
    public class ExtendedImage : Image
    {
        public bool IsUsingSpriteAtlas = true;
        
        protected override void Start()
        {
            base.Start();

            // useSpriteMesh = true;
#if UNITY_EDITOR
            if (sprite == null)
            {
                return;
            }

            var sceneName = SceneManager.GetActiveScene().name;
            var atlases = SpriteAtlasSettings.Instance.GetValidAtlases(sceneName);

            bool isInAtlas = false;
            foreach (var atlas in atlases)
            {
                if (atlas.GetSprite(sprite.name) != null)
                {
                    isInAtlas = true;
                    break;
                }
            }

            if (!isInAtlas)
            {
                Debug.LogError("extended image :: sprite atlas validation failed. object name = " + gameObject.name);
            }
#endif
        }
    }
} 