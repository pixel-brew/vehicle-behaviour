using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using static System.IO.Directory;

namespace Game.Client.Utility
{
    public static class ObjectPreparation
    {
        public const float padding = 5f;
#if UNITY_EDITOR

        [UnityEditor.MenuItem("Assets/Object Preparation/Rename Selected Files")]
        static void UpdateFileNames()
        {
            // string path = UnityEditor.EditorUtility.OpenFolderPanel("Select Folder", "", "");
            // Debug.LogError("path >> " + path + " path >> " + path.Replace(Application.dataPath, ""));  ;
            // var assets = UnityEditor.AssetDatabase.FindAssets("t:prefab", new []{path});

            foreach (var o in UnityEditor.Selection.objects)
            {
                if (o is GameObject)
                {
                    var path = UnityEditor.AssetDatabase.GetAssetPath(o);
                    var name = System.IO.Path.GetFileNameWithoutExtension(path);
                    name = name.Replace("props_00_", "props_");
                    name = name.Replace("props_01_", "props_");
                    UnityEditor.AssetDatabase.RenameAsset(path, name);
                }
            }
        }

        [UnityEditor.MenuItem("Assets/Object Preparation/{}")]
        static void AddPrefix()
        {
            foreach (var o in UnityEditor.Selection.objects)
            {
                if (o is GameObject)
                {
                    var path = UnityEditor.AssetDatabase.GetAssetPath(o);
                    var name = System.IO.Path.GetFileNameWithoutExtension(path);
                    // name = "props_" + name;
                    name = "props_" + name.Replace(" 1", "");

                    UnityEditor.AssetDatabase.RenameAsset(path, name);
                }
            }
        }


        [UnityEditor.MenuItem("Assets/Object Preparation/SetRendererSettings")]
        static void SetRendererSettings()
        {
            var selectedObjects = UnityEditor.Selection.objects;

            foreach (var o in selectedObjects)
            {
                var obj = (GameObject)o;

                var renderers = new List<Renderer>();
                renderers.Add(obj.GetComponent<Renderer>());
                renderers.AddRange(obj.GetComponentsInChildren<Renderer>());
                foreach (var r in renderers)
                {
                    if (r != null)
                    {
                        r.lightProbeUsage = LightProbeUsage.Off;
                        r.allowOcclusionWhenDynamic = false;
                    }
                }
            }
        }

        // [UnityEditor.MenuItem("Tools/Object Preparation/UpdateNameForProps")]
        // static void UpdateNameForProps()
        // {
        //     var selectedObjects = UnityEditor.Selection.objects;
        //
        //     foreach (var o in selectedObjects)
        //     {
        //         var obj = (GameObject)o;
        //         if (obj.name.Contains("props_00_"))
        //         {
        //             obj.name.Replace("props_00_", "");
        //         }
        //
        //         if (obj.name.Contains("props_01_"))
        //         {
        //             obj.name.Replace("props_00_", "");
        //         }
        //
        //         obj.name = "props_" + obj.name;
        //     }
        // }


        private static string[] GetPrefabsFromFolder(string path)
        {
            return GetFiles(path, "*.prefab");
        }

        private static void CopyPrefab(string originPath, string copyPath)
        {
            if (!AssetDatabase.CopyAsset(originPath, copyPath))
            {
                Debug.LogWarning($"Failed to copy {originPath}");
            }
            else
            {
                Debug.Log("make copy" + originPath + " to " + copyPath);
            }
        }


        private static string[] GetAssetsFromFolder(string path)
        {
            return Directory.GetFiles(path, "*.prefab");
        }

        [MenuItem("Tools/Prefabs/MakePrefabsInherited")]
        public static void MakePrefabsInherited()
        {
            var prefabs = GetAssetsFromFolder("Assets/Prefabs/Environment/IndustrialZone/Props/Barrels");
            
            
            var prefabVariant = "Assets/Prefabs/Environment/Base/props_variant.prefab";
            var dstFolder = "Assets/Prefabs/Environment/IndustrialZone/Props/_test";

            foreach (var src in prefabs)
            {
                var prefabName = Path.GetFileName(src);
                var resultPath = Path.Combine(dstFolder, prefabName);

                CopyPrefab(prefabVariant, resultPath);
                MakePrefabsInherited(src, resultPath);
            }
        }

        private static void MakePrefabsInherited(string srcPrefabPath, string dstPrefabPath)
        {
            var dstGameObject = (GameObject)AssetDatabase.LoadMainAssetAtPath(dstPrefabPath);
            var srcGameObject = (GameObject)AssetDatabase.LoadMainAssetAtPath(srcPrefabPath);

            var dst = (GameObject)PrefabUtility.InstantiatePrefab(dstGameObject);
            var src = (GameObject)PrefabUtility.InstantiatePrefab(srcGameObject);

            // copy root
            var components = src.GetComponents<Component>();
            foreach (var component in components)
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(dst);
            }

            PrefabUtility.UnpackPrefabInstance(src, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            var children = new List<GameObject>();
            // copy all children
            try
            {
                foreach (Transform child in src.transform)
                {
                    children.Add(child.gameObject);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

            foreach (GameObject child in children)
            {
                child.transform.parent = dst.GetComponent<Transform>();
            }

            PrefabUtility.ApplyPrefabInstance(dst, InteractionMode.AutomatedAction);

            // Clean up
            GameObject.DestroyImmediate(dst);
            GameObject.DestroyImmediate(src);
        }


        [UnityEditor.MenuItem("Tools/Object Preparation/Place Objects (0 deg)")]
        static void PlaceObjects0()
        {
            PlaceObjects(0f, "_a00");
        }

        [UnityEditor.MenuItem("Tools/Object Preparation/Place Objects (45 deg)")]
        static void PlaceObjects45()
        {
            PlaceObjects(45f, "_a45");
        }

        [UnityEditor.MenuItem("Tools/Object Preparation/Place Objects (90 deg)")]
        static void PlaceObjects90()
        {
            PlaceObjects(90f, "_a90");
        }

        static void PlaceObjects(float rotationAngle, string namePostfix)
        {
            var selectedObjects = UnityEditor.Selection.objects;

            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            foreach (var o in selectedObjects)
            {
                var obj = (GameObject)o;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                var bounds = GetTotalBounds(obj);
                position.x += Math.Max(padding, bounds.size.y * 0.5f) + bounds.size.x;
                obj.name += namePostfix;
            }
        }


        private static Bounds GetTotalBounds(GameObject obj)
        {
            var renderers = new List<Renderer>();
            renderers.Add(obj.GetComponent<Renderer>());
            renderers.AddRange(obj.GetComponentsInChildren<Renderer>());
            Bounds result = new Bounds(obj.transform.position, Vector3.zero);
            foreach (var r in renderers)
            {
                if (r != null)
                {
                    result.Encapsulate(r.bounds);
                }
            }

            return result;
        }
#endif
    }
}