using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DG.DemiEditor;
using Template.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Template.Editor
{
    [CustomEditor(typeof(SavesDataObjectJson))]
    public class SavesObjectJosnEditor : EditorTweaks
    {
        SavesDataObjectJson save;


        [MenuItem("Template/Open Saves")]
        public static void OpenSavePath()
        {
            Application.OpenURL(Application.persistentDataPath);
        }
        
        [MenuItem("Template/Delete Saves")]
        public static void DeleteSavePath()
        {
            string _path = Application.persistentDataPath + "/Save.json";

            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
        
        public enum BoolEnum
        {
            True = 1,
            False = 0
        };

        public void OnEnable()
        {
            save = (SavesDataObjectJson) target;
        }

        public void OnDisable()
        {
            save.Save();
            Save(save);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (save.prefsData == null)
            {
                save.LoadCheck();
            }

            DrawSeparator();
            if (GUILayout.Button("Clear Player Prefs"))
            {
                save.Clear();
            }


            DrawSeparator();
            GUILayout.Label("Active Prefs:");
            
            
            Rect scale = GUILayoutUtility.GetLastRect();
            
            EditorGUIUtility.LookLikeInspector();

            
            var leftSide = new GUILayoutOption[] {GUILayout.MinHeight(EditorGUIUtility.singleLineHeight)};
            var middleSide = new GUILayoutOption[] {GUILayout.MinWidth(120)};
            var rightSide = new GUILayoutOption[]  {};
            var buttonSide = new GUILayoutOption[] { };
            
            GUILayout.BeginHorizontal();
            {
                GUI.enabled = true;
                
                GUILayout.BeginVertical();
                {
                    SpawnLabels(leftSide);
                }
                GUILayout.EndVertical();
                
                GUILayout.BeginVertical();
                {
                    SpawnPrefTypes(middleSide);
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    SpawnFields(rightSide);
                }
                GUILayout.EndVertical();

                
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }

        public void SpawnPrefTypes(GUILayoutOption[] rightSide)
        {
            foreach (var item in save.prefsData)
            {
                var old = item.Value.type;
                item.Value.type = (PrefType)EditorGUILayout.EnumPopup(item.Value.type, rightSide);
                if (old != item.Value.type)
                {
                    Save(save);
                    save.Save();
                }
            }

        }
        
        public void SpawnLabels(GUILayoutOption[] leftSide)
        {
            foreach (var item in save.prefsData)
            {
                GUILayout.Label(AddSpacesToSentence(item.Key.ToString()) + ": ", leftSide);
            }
        }
        
        public void SpawnFields(GUILayoutOption[] middleSide)
        {
            foreach (var item in save.prefsData)
            {
                if (item.Key == Prefs.CompletedLevels || item.Key == Prefs.Level || item.Key == Prefs.StartsCount)
                {
                    GUI.enabled = false;
                }

                switch (item.Value.type)
                {
                    case PrefType.String:
                        if (save.GetPref(item.Key) == null)
                        {
                            save.SetPref(item.Key, "");
                        }
                        save.SetPref(item.Key, GUILayout.TextField(save.GetPref(item.Key).ToString(), middleSide));
                        break;
                    case PrefType.Int:
                        save.SetPref(item.Key, EditorGUILayout.IntField((int) save.GetPref(item.Key), middleSide));
                        break;
                    case PrefType.Float:
                        save.SetPref(item.Key, EditorGUILayout.FloatField((float) save.GetPref(item.Key), middleSide));
                        break;
                    case PrefType.Bool:
                        var _bool = (bool) save.GetPref(item.Key) == true ? 1 : 0;
                        BoolEnum boolenum = (BoolEnum) _bool;
                        boolenum = (BoolEnum) EditorGUILayout.EnumPopup(boolenum);
                        save.SetPref(item.Key, boolenum == BoolEnum.True ? true : false);
                        break;
                }

                if (item.Key == Prefs.CompletedLevels || item.Key == Prefs.Level)
                {
                    if (item.Value.type != PrefType.Int)
                    {
                        item.Value.type = PrefType.Int;
                    }
                }

                GUI.enabled = true;
            }

        }



        string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
