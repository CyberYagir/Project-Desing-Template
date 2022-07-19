using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Template.Editor
{
    public class PrefabFinderWindow : EditorWindow
    {
        public static List<GameObject> prefabs;
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PrefabFinderWindow));
            EditorWindow.GetWindow(typeof(PrefabFinderWindow)).titleContent = new GUIContent("Finded Paticles");
        }

        public void OnGUI()
        {
            if (prefabs != null)
            {
                for (int i = 0; i < prefabs.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUI.enabled = false;
#pragma warning disable 618
                        EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject));
#pragma warning restore 618
                        GUI.enabled = true;
                        if (GUILayout.Button("Open Prefab"))
                        {
                            PrefabFinderEditor.PingAssetByObject(prefabs[i]);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
    public class PrefabFinderEditor : UnityEditor.Editor
    {
        [MenuItem("Template/Find Prefab")]
        public static void FindFileByName()
        {
            if (Selection.activeTransform != null)
            {
                var obj = Selection.activeTransform.gameObject;
                var particlesList = new List<ParticleSystem>();
                if (obj.GetComponent<ParticleSystem>() && obj.GetComponentInParent<ParticleSystem>(true))
                {
                    particlesList.Add(obj.GetComponent<ParticleSystem>());
                    particlesList = obj.GetComponentsInParent<ParticleSystem>(true).ToList();
                }


                if (particlesList.Count == 0)
                {
                    var name = RefactorName(obj.name);
                    var assets = AssetDatabase.FindAssets(name + " t:Prefab");
                    if (assets.Length >= 1)
                    {
                        PingAssetByPath(assets[0]);
                    }
                }
                else
                {
                    var list = new List<GameObject>();
                    foreach (var parts in particlesList)
                    {
                        var assets = AssetDatabase.FindAssets(RefactorName(parts.transform.name) + " t:Prefab");
                        if (assets.Length != 0)
                        {
                            var file = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assets[0]));
                            if (file.name == RefactorName(parts.transform.name))
                            {
                                list.Add(file.gameObject);
                            }
                        }
                    }

                    
                    if (list.Count > 1)
                    {
                        PrefabFinderWindow.prefabs = list;
                        PrefabFinderWindow.ShowWindow();
                    }
                    else if (list.Count == 1)
                    {
                        PingAssetByObject(list[0]);
                    }
                }
            }
        }


        public static void PingAssetByPath(string assetPath)
        {
            var file = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetPath));
            if (file != null)
            {
                EditorGUIUtility.PingObject(file);
                AssetDatabase.OpenAsset(file.gameObject);
            }

        }
        
        public static void PingAssetByObject(GameObject file)
        {
            if (file != null)
            {
                EditorGUIUtility.PingObject(file);
                AssetDatabase.OpenAsset(file.gameObject);
            }

        }
        public static string RefactorName(string name)
        {
            name = name.Replace("(Clone)", "");
            for (int i = 0; i < 10; i++)
            {
                name = name.Replace("(" + i + ")", "");
            }
            name = name.Split('.')[0];
            name = name.Trim();

            return name;
        }
    }
}
