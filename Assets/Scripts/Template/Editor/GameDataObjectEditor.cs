using System.Collections.Generic;
using System.Linq;
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
            gameData.main.levelList = gameData.main.levelList.OrderBy(x => x.name).ToList();
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

                if (GUILayout.Button(GameDataObject.DebugLevel.IsDebugLevel ? "Disable Debug" : "Enable Debug"))
                {
                    GameDataObject.DebugLevel.IsDebugLevel = !GameDataObject.DebugLevel.IsDebugLevel;
                }
                GUILayout.BeginHorizontal();
                if (GameDataObject.DebugLevel.IsDebugLevel)
                {
                    GUILayout.Label("Debug Level: ", GUILayout.MaxWidth(85));
                    List<string> levels = new List<string>();
                    foreach (var it in gameData.main.levelList)
                    {
                        levels.Add(it.name);
                    }

                    GameDataObject.DebugLevel.LevelID = EditorGUILayout.Popup("", GameDataObject.DebugLevel.LevelID,
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
