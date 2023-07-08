using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Client.Utility
{
    // https://github.com/Ayfel/PrefabLightmapping
    // 
    // Script for saving lightmapping data to prefabs. Used through the Assets tab in Unity.
    // Place your prefabs in the scene with this script at the root. Set up your lighting and
    // in the editor go to Assets->Bake Prefab Lightmaps. After the bake is processed you can
    // now spawn your prefabs in different scenes and they will use the lightmapping from
    // the original scene.
    //
    // Remember that if you are not instantiating your prefabs at runtime you should remove
    // the static flag from the GameObjects, otherwise static batching will mess with uvs and
    // the lightmap won't work properly.
    //
    // If you find problems when building make sure to check your graphics settings under Project Settings,
    // as shader stripping might be the cause of the issue. Try playing with the option "Lightmap Modes"
    // and setting it to Custom if it's not working.
    
    [ExecuteAlways]
    public class PrefabLightmapData : MonoBehaviour
    {
        [System.Serializable]
        struct RendererInfo
        {
            public Renderer Renderer;
            public int LightmapIndex;
            public Vector4 LightmapOffsetScale;
        }

        [System.Serializable]
        struct LightInfo
        {
            public Light Light;
            public int LightmapBakeType;
            public int MixedLightingMode;
        }

        [FormerlySerializedAs("_RendererInfo")] [FormerlySerializedAs("m_RendererInfo")] [SerializeField]
        RendererInfo[] _rendererInfo;

        [FormerlySerializedAs("m_Lightmaps")] [SerializeField]
        Texture2D[] _lightmaps;

        [FormerlySerializedAs("m_LightmapsDir")] [SerializeField]
        Texture2D[] _lightmapsDir;

        [FormerlySerializedAs("m_ShadowMasks")] [SerializeField]
        Texture2D[] _shadowMasks;

        [FormerlySerializedAs("m_LightInfo")] [SerializeField]
        LightInfo[] _lightInfo;


        void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_rendererInfo == null || _rendererInfo.Length == 0)
                return;

            var lightmaps = LightmapSettings.lightmaps;
            int[] offsetsindexes = new int[_lightmaps.Length];
            int counttotal = lightmaps.Length;
            List<LightmapData> combinedLightmaps = new List<LightmapData>();

            for (int i = 0; i < _lightmaps.Length; i++)
            {
                bool exists = false;
                for (int j = 0; j < lightmaps.Length; j++)
                {

                    if (_lightmaps[i] == lightmaps[j].lightmapColor)
                    {
                        exists = true;
                        offsetsindexes[i] = j;

                    }

                }

                if (!exists)
                {
                    offsetsindexes[i] = counttotal;
                    var newlightmapdata = new LightmapData
                    {
                        lightmapColor = _lightmaps[i],
                        lightmapDir = _lightmapsDir.Length == _lightmaps.Length ? _lightmapsDir[i] : default(Texture2D),
                        shadowMask = _shadowMasks.Length == _lightmaps.Length ? _shadowMasks[i] : default(Texture2D),
                    };

                    combinedLightmaps.Add(newlightmapdata);

                    counttotal += 1;


                }

            }

            var combinedLightmaps2 = new LightmapData[counttotal];

            lightmaps.CopyTo(combinedLightmaps2, 0);
            combinedLightmaps.ToArray().CopyTo(combinedLightmaps2, lightmaps.Length);

            bool directional = true;

            foreach (Texture2D t in _lightmapsDir)
            {
                if (t == null)
                {
                    directional = false;
                    break;
                }
            }

            LightmapSettings.lightmapsMode = (_lightmapsDir.Length == _lightmaps.Length && directional) ? LightmapsMode.CombinedDirectional : LightmapsMode.NonDirectional;
            ApplyRendererInfo(_rendererInfo, offsetsindexes, _lightInfo);
            LightmapSettings.lightmaps = combinedLightmaps2;
        }

        void OnEnable()
        {

            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Init();
        }

        // called when the game is terminated
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }



        static void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapOffsetIndex, LightInfo[] lightsInfo)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                var info = infos[i];

                info.Renderer.lightmapIndex = lightmapOffsetIndex[info.LightmapIndex];
                info.Renderer.lightmapScaleOffset = info.LightmapOffsetScale;

                // You have to release shaders.
                Material[] mat = info.Renderer.sharedMaterials;
                for (int j = 0; j < mat.Length; j++)
                {
                    if (mat[j] != null && Shader.Find(mat[j].shader.name) != null)
                        mat[j].shader = Shader.Find(mat[j].shader.name);
                }

            }

            for (int i = 0; i < lightsInfo.Length; i++)
            {
                LightBakingOutput bakingOutput = new LightBakingOutput();
                bakingOutput.isBaked = true;
                bakingOutput.lightmapBakeType = (LightmapBakeType)lightsInfo[i].LightmapBakeType;
                bakingOutput.mixedLightingMode = (MixedLightingMode)lightsInfo[i].MixedLightingMode;

                lightsInfo[i].Light.bakingOutput = bakingOutput;

            }


        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Lightmap/Bake Prefabs")]
        static void GenerateLightmapInfo()
        {
            if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
            {
                Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
                return;
            }

            UnityEditor.EditorUtility.DisplayProgressBar("Baking", "Lightmapping bake", 0.2f);
            UnityEditor.Lightmapping.Bake();

            PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();

            int prefabsCounter = 0;
            foreach (var instance in prefabs)
            {
                ++prefabsCounter;

                var gameObject = instance.gameObject;
                var rendererInfos = new List<RendererInfo>();
                var lightmaps = new List<Texture2D>();
                var lightmapsDir = new List<Texture2D>();
                var shadowMasks = new List<Texture2D>();
                var lightsInfos = new List<LightInfo>();

                GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, lightmapsDir, shadowMasks, lightsInfos);

                instance._rendererInfo = rendererInfos.ToArray();
                instance._lightmaps = lightmaps.ToArray();
                instance._lightmapsDir = lightmapsDir.ToArray();
                instance._lightInfo = lightsInfos.ToArray();
                instance._shadowMasks = shadowMasks.ToArray();
#if UNITY_2018_3_OR_NEWER
                var targetPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance.gameObject) as GameObject;
                if (targetPrefab != null)
                {
                    GameObject root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(instance.gameObject); // 根结点
                    //如果当前预制体是是某个嵌套预制体的一部分（IsPartOfPrefabInstance）
                    if (root != null)
                    {
                        GameObject rootPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(instance.gameObject);
                        string rootPath = UnityEditor.AssetDatabase.GetAssetPath(rootPrefab);
                        //打开根部预制体
                        UnityEditor.PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, UnityEditor.PrefabUnpackMode.OutermostRoot);
                        try
                        {
                            //Apply各个子预制体的改变
                            UnityEditor.PrefabUtility.ApplyPrefabInstance(instance.gameObject, UnityEditor.InteractionMode.AutomatedAction);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            //重新更新根预制体
                            UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, UnityEditor.InteractionMode.AutomatedAction);
                        }
                    }
                    else
                    {
                        UnityEditor.PrefabUtility.ApplyPrefabInstance(instance.gameObject, UnityEditor.InteractionMode.AutomatedAction);
                    }
                }
#else
            var targetPrefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
            if (targetPrefab != null)
            {
                //UnityEditor.Prefab
                UnityEditor.PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
            }
#endif
            }

            UnityEditor.EditorUtility.ClearProgressBar();
        }

        static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps, List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks, List<LightInfo> lightsInfo)
        {
            var renderers = root.GetComponentsInChildren<MeshRenderer>();
            int renderersCounter = 0;
            foreach (MeshRenderer renderer in renderers)
            {
                renderersCounter++;
                UnityEditor.EditorUtility.DisplayProgressBar("Baking", $"Baking prefabs {renderersCounter} / {renderers.Length}", 0.2f + 0.8f * (float)renderersCounter / (float)renderers.Length);
                if (renderer.lightmapIndex != -1)
                {
                    RendererInfo info = new RendererInfo();
                    info.Renderer = renderer;

                    if (renderer.lightmapScaleOffset != Vector4.zero)
                    {
                        //1ibrium's pointed out this issue : https://docs.unity3d.com/ScriptReference/Renderer-lightmapIndex.html
                        if (renderer.lightmapIndex < 0 || renderer.lightmapIndex == 0xFFFE) continue;
                        info.LightmapOffsetScale = renderer.lightmapScaleOffset;

                        Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
                        Texture2D lightmapDir = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapDir;
                        Texture2D shadowMask = LightmapSettings.lightmaps[renderer.lightmapIndex].shadowMask;

                        info.LightmapIndex = lightmaps.IndexOf(lightmap);
                        if (info.LightmapIndex == -1)
                        {
                            info.LightmapIndex = lightmaps.Count;
                            lightmaps.Add(lightmap);
                            lightmapsDir.Add(lightmapDir);
                            shadowMasks.Add(shadowMask);
                        }

                        rendererInfos.Add(info);
                    }

                }
            }

            var lights = root.GetComponentsInChildren<Light>(true);

            foreach (Light l in lights)
            {
                LightInfo lightInfo = new LightInfo();
                lightInfo.Light = l;
                lightInfo.LightmapBakeType = (int)l.lightmapBakeType;
#if UNITY_2020_1_OR_NEWER
                lightInfo.MixedLightingMode = (int)UnityEditor.Lightmapping.lightingSettings.mixedBakeMode;
#elif UNITY_2018_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)UnityEditor.LightmapEditorSettings.mixedBakeMode;
#else
            lightInfo.mixedLightingMode = (int)l.bakingOutput.lightmapBakeType;
#endif
                lightsInfo.Add(lightInfo);

            }
        }
#endif

    }
}