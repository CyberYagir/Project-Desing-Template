using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Template.Editor;
using Template.Scriptable;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SaveDataObjectJson))]
public class SaveDataJsonEditor : EditorTweaks
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawSeparator();

        var data = (target as SaveDataObjectJson);
        
        if (GUILayout.Button("Reset"))
        {
            data.ResetObject();
        }

        string currentJson = "";
        string savedJson = "";
        var path = Application.persistentDataPath + "/Save.json";
        if (File.Exists(path))
        {
            savedJson = File.ReadAllText(path);
            currentJson = JsonUtility.ToJson(data);
        }

        GUI.enabled = currentJson != savedJson;
        if (GUILayout.Button("Save"))
        {
            data.Save();
        }

        GUI.enabled = true;
    }

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

    private void OnEnable()
    {
        (target as SaveDataObjectJson)?.Load();
    }

    // private void OnDisable()
    // {
    //     (target as SaveDataObjectJson)?.Save();
    // }
}
