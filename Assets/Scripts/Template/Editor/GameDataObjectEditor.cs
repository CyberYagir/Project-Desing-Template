using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

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
    }
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        DrawSeparator();
        if (GameDataObject.GetData(true) == gameData)
        {
            gameData.main.saves = (AbstractSavesDataObject)EditorGUILayout.ObjectField("Saves: ", gameData.main.saves, typeof(AbstractSavesDataObject), false, GUILayout.MinWidth(50));

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
                gameData.main.levelList[i] = (LevelManager)EditorGUILayout.ObjectField("", gameData.main.levelList[i], typeof(LevelManager), false, GUILayout.MinWidth(50));

                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        var th = gameData.main.levelList[i - 1];
                        gameData.main.levelList[i - 1] = gameData.main.levelList[i];
                        gameData.main.levelList[i] = th;
                    }
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
                    return;
                }

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    gameData.main.levelList.RemoveAt(i);
                    return;
                }
                GUILayout.EndHorizontal();
            }

            DrawSeparator();
        }
        

        if (GUILayout.Button("Find all objects"))
        {
            gameData.main = Configurator.GetAllDataFromAssets();
            gameData.main.saves = Resources.LoadAll<AbstractSavesDataObject>("")[0];
            Sort();
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
