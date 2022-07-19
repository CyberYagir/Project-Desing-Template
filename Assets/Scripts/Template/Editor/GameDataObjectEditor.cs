using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Template.Managers;
using Template.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Template.Editor
{
    [CustomEditor(typeof(GameDataObject))]
    public class GameDataObjectEditor : EditorTweaks
    {
        GameDataObject gameData;
        public void OnEnable()
        {
            gameData = (GameDataObject)target;
        }
        public void OnDisable()
        {
            gameData.MainData.levelList.RemoveAll(x => x == null);
            Save(gameData);
        }
        public void Sort()
        {
            gameData.MainData.levelList.RemoveAll(x => x == null);
            gameData.MainData.levelList = gameData.MainData.levelList.OrderBy(x => int.Parse(Regex.Match(x.name, @"\d+").Value)).ToList();
            Save(gameData);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawSeparator();
            if (GameDataObject.StaticGetStandardData() == null)
            {
                GUILayout.Label("Create DataManager");
                return;
            }
            if (GameDataObject.StaticGetStandardData() == gameData)
            {
                DrawMainGameData();
                DrawSeparator();

                
                if (GUILayout.Button(gameData.DebugLevel.isDebugLevel ? "Disable Debug" : "Enable Debug"))
                {
                    gameData.DebugLevel.isDebugLevel = !gameData.DebugLevel.isDebugLevel;
                    Save(this);
                }

                GUILayout.BeginHorizontal();
                if (gameData.DebugLevel.isDebugLevel)
                {
                    GUILayout.Label("Debug Level: ", GUILayout.MaxWidth(85));
                    List<string> levels = new List<string>();
                    foreach (var it in gameData.MainData.levelList)
                    {
                        levels.Add(it.name);
                    }

                    gameData.DebugLevel.levelID = EditorGUILayout.Popup("", gameData.DebugLevel.levelID, levels.ToArray(), GUILayout.MinWidth(80));
                }

                GUILayout.EndHorizontal();
                DrawSeparator();
            }


            // if (GUILayout.Button("Find all objects"))
            // {
            //     gameData.SetData(ConfiguratorWindow.GetAllDataFromAssets(), Resources.LoadAll<AbstractSavesDataObject>("")[0]);
            //     Sort();
            // }
        }



        public void DrawMainGameData()
        {
            
            gameData.SetSaves((AbstractSavesDataObject) EditorGUILayout.ObjectField("Saves: ", gameData.Saves, typeof(AbstractSavesDataObject), false, GUILayout.MinWidth(50)));

            GUILayout.Label("Levels List: ");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Level"))
            {
                gameData.MainData.levelList.Add(null);
            }

            if (GUILayout.Button("Autofind Levels"))
            {
                gameData.MainData.levelList = FindLevels();
                Sort();
            }

            if (GUILayout.Button("Sort"))
            {
                Sort();
            }

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                gameData.MainData.levelList.RemoveAll(x => x == null);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("main").FindPropertyRelative("levelList"), true);
                if (serializedObject.FindProperty("main").FindPropertyRelative("levelList").isExpanded)
                {
                    GUILayout.BeginVertical(GUILayout.MaxWidth(20));
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight / 2f);
                        for (int i = 0; i < gameData.MainData.levelList.Count; i++)
                        {
                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                gameData.MainData.levelList.RemoveAt(i);
                                Save(gameData);
                                return;
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
            
        }

        public void DrawCustomLevelList()
        {
            for (int i = 0; i < gameData.MainData.levelList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(i + ": ", GUILayout.MaxWidth(15));
                var old = gameData.MainData.levelList[i];
                gameData.MainData.levelList[i] = (LevelManager) EditorGUILayout.ObjectField("",
                    gameData.MainData.levelList[i], typeof(LevelManager), false, GUILayout.MinWidth(50));
                if (old != gameData.MainData.levelList[i])
                {
                    Save(gameData);
                }

                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        (gameData.MainData.levelList[i - 1], gameData.MainData.levelList[i]) = (gameData.MainData.levelList[i], gameData.MainData.levelList[i - 1]);
                    }

                    Save(gameData);

                    return;
                }

                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != gameData.MainData.levelList.Count - 1)
                    {
                        (gameData.MainData.levelList[i + 1], gameData.MainData.levelList[i]) = (gameData.MainData.levelList[i], gameData.MainData.levelList[i + 1]);
                    }

                    Save(gameData);
                    return;
                }

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    gameData.MainData.levelList.RemoveAt(i);
                    Save(gameData);
                    return;
                }

                GUILayout.EndHorizontal();
            }
        }

        public List<LevelLogic> FindLevels()
        {
            var prefabs = AssetDatabase.FindAssets("t:prefab");
            var n = new List<LevelLogic>();
            foreach (var prefab in prefabs)
            {
                var level = AssetDatabase.LoadAssetAtPath<LevelLogic>(AssetDatabase.GUIDToAssetPath(prefab));
                if (level != null)
                {
                    n.Add(level);
                }
            }
            return n;
        }
    }
}
