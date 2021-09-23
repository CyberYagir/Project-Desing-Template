using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SavesDataObject))]
public class SavesObjectEditor : EditorTweaks
{
    SavesDataObject save;

    public enum BoolEnum {True = 1, False = 0};

    public void OnEnable()
    {
        save = (SavesDataObject)target;
    }
    public void OnDisable()
    {
        Save(save);
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

        var prefs = Enum.GetValues(typeof(Prefs)).Cast<Prefs>().ToList();
        var notActivePrefs = new List<Prefs>();

        for (int i = 0; i < prefs.Count; i++)
        {
            if (save.prefsValues.Find(x => x.pref.ToString() == prefs[i].ToString()) == null)
            {
                notActivePrefs.Add(prefs[i]);
            }
        }

        GUILayout.Label("Not Active Prefs:");
        if (notActivePrefs.Count != 0)
        {
            foreach (var item in notActivePrefs)
            {
                if (GUILayout.Button(item.ToString()))
                {
                    save.prefsValues.Add(new PrefsValue() { pref = item, savePref = PrefType.Int });
                    //Save(save);
                }
            }
        }
        else
        {
            GUILayout.Label("All Prefs is active.");
        }

        DrawSeparator();
        GUILayout.Label("Active Prefs:");
        foreach (var item in save.prefsValues)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(AddSpacesToSentence(item.pref.ToString()) + ": ", GUILayout.MinWidth(110));



            switch (item.savePref)
            {
                case PrefType.String:
                    save.SetPref(item.pref, GUILayout.TextField(save.GetPref(item.pref).ToString()));
                    break;
                case PrefType.Int:
                    save.SetPref(item.pref, EditorGUILayout.IntField((int)save.GetPref(item.pref)));
                    break;
                case PrefType.Float:
                    save.SetPref(item.pref, EditorGUILayout.FloatField((float)save.GetPref(item.pref)));
                    break;
                case PrefType.Bool:
                    var _bool = (bool)save.GetPref(item.pref) == true ? 1 : 0;
                    BoolEnum boolenum = (BoolEnum)_bool;
                    boolenum = (BoolEnum)EditorGUILayout.EnumPopup(boolenum);
                    save.SetPref(item.pref, boolenum == BoolEnum.True ? true : false);
                    break;
            }
            if (item.pref == Prefs.CompletedLevels || item.pref == Prefs.Level)
            {
                if (item.savePref != PrefType.Int)
                {
                    item.savePref = PrefType.Int;
                }
                GUI.enabled = false;
            }
            var old = item.savePref;
            item.savePref = (PrefType)EditorGUILayout.EnumPopup(item.savePref, GUILayout.MinWidth(50));
            GUI.enabled = true;
            if (old != item.savePref)
            {
                //Save(save);
            }

            if (item.pref == Prefs.CompletedLevels || item.pref == Prefs.Level)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                save.prefsValues.Remove(item);
                //Save(save);
                return;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
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
