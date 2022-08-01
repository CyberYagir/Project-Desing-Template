﻿using System.Collections.Generic;
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
        // public void OnDisable()
        // {
        //     RemoveAllNull();
        //     Save(gameData);
        // }

        public void RemoveAllNull()
        {
            var levels = gameData.Levels;
            levels.RemoveAll(x => x == null);
            gameData.SetLevels(levels);
        }
        public void Sort()
        {
            RemoveAllNull();
            var levels = gameData.Levels;
            levels = levels.OrderBy(x => int.Parse(Regex.Match(x.name, @"\d+").Value)).ToList();
            gameData.SetLevels(levels);
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
                    foreach (var it in gameData.Levels)
                    {
                        levels.Add(it.name);
                    }

                    gameData.DebugLevel.levelID = EditorGUILayout.Popup("", gameData.DebugLevel.levelID, levels.ToArray(), GUILayout.MinWidth(80));
                }

                GUILayout.EndHorizontal();
                DrawSeparator();
            }


            if (GUILayout.Button("Find all objects"))
            {
                gameData.SetData(ConfiguratorWindow.GetAllDataFromAssets(), Resources.LoadAll<AbstractSavesDataObject>("")[0]);
                Sort();
            }
        }



        public void DrawMainGameData()
        {
            
            gameData.SetSaves((AbstractSavesDataObject) EditorGUILayout.ObjectField("Saves: ", gameData.Saves, typeof(AbstractSavesDataObject), false, GUILayout.MinWidth(50)));
            gameData.SetSound((SoundDataObject) EditorGUILayout.ObjectField("Sounds: ", gameData.Sound, typeof(SoundDataObject), false, GUILayout.MinWidth(50)));

            DrawSeparator();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Level"))
            {
                RemoveAllNull();
                var levels = gameData.Levels;
                levels.Add(null);
                gameData.SetLevels(levels);
            }

            if (GUILayout.Button("Autofind Levels"))
            {
                gameData.SetLevels(FindLevels());
                Sort();
            }

            if (GUILayout.Button("Sort"))
            {
                Sort();
            }

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("levelList"));
                if (serializedObject.FindProperty("levelList").isExpanded)
                {
                    GUILayout.BeginVertical(GUILayout.MaxWidth(20));
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight / 2f);
                        var levels = gameData.Levels;
                        for (int i = 0; i < levels.Count; i++)
                        {
                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                levels.RemoveAt(i);
                                gameData.SetLevels(levels);
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
