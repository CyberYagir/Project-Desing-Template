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
            gameData.main.levelList.RemoveAll(x => x == null);
            Save(gameData);
        }
        public void Sort()
        {
            gameData.main.levelList.RemoveAll(x => x == null);
            gameData.main.levelList = gameData.main.levelList.OrderBy(x => int.Parse(Regex.Match(x.name, @"\d+").Value)).ToList();
            Save(gameData);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawSeparator();
            if (GameDataObject.GetData(true) == gameData)
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
                    foreach (var it in gameData.main.levelList)
                    {
                        levels.Add(it.name);
                    }

                    gameData.DebugLevel.levelID = EditorGUILayout.Popup("", gameData.DebugLevel.levelID,
                        levels.ToArray(), GUILayout.MinWidth(80));

                }
                GUILayout.EndHorizontal();

                DrawSeparator();
            }


            if (GUILayout.Button("Find all objects"))
            {
                gameData.main = ConfiguratorWindow.GetAllDataFromAssets();
                gameData.main.saves = Resources.LoadAll<AbstractSavesDataObject>("")[0];
                Sort();
            }
        }



        public void DrawMainGameData()
        {
            gameData.main.saves = (AbstractSavesDataObject) EditorGUILayout.ObjectField("Saves: ",
                gameData.main.saves, typeof(AbstractSavesDataObject), false, GUILayout.MinWidth(50));

            GUILayout.Label("Levels List: ");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Level"))
            {
                gameData.main.levelList.Add(null);
            }

            if (GUILayout.Button("Autofind Levels"))
            {
                gameData.main.levelList = FindLevels();
                Sort();
            }

            if (GUILayout.Button("Sort"))
            {
                Sort();
            }

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                gameData.main.levelList.RemoveAll(x => x == null);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("main").FindPropertyRelative("levelList"), true);
                if (serializedObject.FindProperty("main").FindPropertyRelative("levelList").isExpanded)
                {
                    GUILayout.BeginVertical(GUILayout.MaxWidth(20));
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight / 2f);
                        for (int i = 0; i < gameData.main.levelList.Count; i++)
                        {
                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                gameData.main.levelList.RemoveAt(i);
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
            for (int i = 0; i < gameData.main.levelList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(i + ": ", GUILayout.MaxWidth(15));
                var old = gameData.main.levelList[i];
                gameData.main.levelList[i] = (LevelManager) EditorGUILayout.ObjectField("",
                    gameData.main.levelList[i], typeof(LevelManager), false, GUILayout.MinWidth(50));
                if (old != gameData.main.levelList[i])
                {
                    Save(gameData);
                }

                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        var th = gameData.main.levelList[i - 1];
                        gameData.main.levelList[i - 1] = gameData.main.levelList[i];
                        gameData.main.levelList[i] = th;
                    }

                    Save(gameData);

                    return;
                }

                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != gameData.main.levelList.Count - 1)
                    {
                        var th = gameData.main.levelList[i + 1];
                        gameData.main.levelList[i + 1] = gameData.main.levelList[i];
                        gameData.main.levelList[i] = th;
                    }

                    Save(gameData);
                    return;
                }

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    gameData.main.levelList.RemoveAt(i);
                    Save(gameData);
                    return;
                }

                GUILayout.EndHorizontal();
            }
        }

        public List<LevelManager> FindLevels()
        {
            var prefabs = AssetDatabase.FindAssets("t:prefab");
            var n = new List<LevelManager>();
            foreach (var prefab in prefabs)
            {
                var level = AssetDatabase.LoadAssetAtPath<LevelManager>(AssetDatabase.GUIDToAssetPath(prefab));
                if (level != null)
                {
                    n.Add(level);
                }
            }
            return n;
        }
    }
}
