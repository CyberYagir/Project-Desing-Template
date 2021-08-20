using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SavesDataObject))]
public class SavesObjectEditor : Editor
{
    SavesDataObject save;

    public enum BoolEnum {True = 1, False = 0};

    public void OnEnable()
    {
        save = (SavesDataObject)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        save.SetNames();
        DrawSeparator();
        if (GUILayout.Button("Clear Player Prefs"))
        {
            PlayerPrefs.DeleteAll();
        }
        DrawSeparator();
        GUILayout.Label("List of Prefs:");
        List<Prefs> drawedPrefs = new List<Prefs>();
        foreach (var item in save.prefsValues)
        {
            if (!drawedPrefs.Contains(item.pref))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(item.pref.ToString(), GUILayout.Width(50));
                switch (item.savePref)
                {
                    case PrefType.String:
                        save.SetValue(item.pref, GUILayout.TextField(save.GetFromPrefs(item.pref).ToString()));
                        break;
                    case PrefType.Int:
                        save.SetValue(item.pref, EditorGUILayout.IntField((int)save.GetFromPrefs(item.pref)));
                        break;
                    case PrefType.Float:
                        save.SetValue(item.pref, EditorGUILayout.FloatField((float)save.GetFromPrefs(item.pref)));
                        break;
                    case PrefType.Bool:
                        var _bool = (bool)save.GetFromPrefs(item.pref) == true ? 1 : 0;
                        BoolEnum boolenum = (BoolEnum)_bool;
                        boolenum = (BoolEnum)EditorGUILayout.EnumPopup(boolenum);
                        save.SetValue(item.pref, boolenum == BoolEnum.True ? true : false);
                        break;
                }
                GUILayout.EndHorizontal();
                drawedPrefs.Add(item.pref);
            }
        }
    }
    void DrawSeparator()
    {
        GUILayout.Space(10);
        DrawLine();
        GUILayout.Space(10);
    }
    void DrawLine(int h = 1)

    {

        Rect rect = EditorGUILayout.GetControlRect(false, h);

        rect.height = h;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

    }

}
