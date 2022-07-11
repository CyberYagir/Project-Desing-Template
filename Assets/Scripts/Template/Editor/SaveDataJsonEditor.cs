using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Template.Scriptable;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SaveDataObjectJson))]
public class SaveDataJsonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset"))
        {
            (target as SaveDataObjectJson).ResetObject();
        }
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

    private void OnDisable()
    {
        (target as SaveDataObjectJson)?.Save();
    }
}
