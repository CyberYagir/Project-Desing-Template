using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDatasManagerObject))]
public class GameTypesObjectEditor : EditorTweaks
{
    GameDatasManagerObject types;

    public void OnEnable()
    {
        types = (GameDatasManagerObject)target;
    }

    public void OnDisable()
    {
        Save(types);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawSeparator();
        if (GUILayout.Button("Add Mode"))
        {
            types.gameDatas.Add(new LevelAndGameData());
            //Save(types);
        }
        DrawSeparator();
        for (int i = 0; i < types.gameDatas.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Game Mode [{i}]: ", GUILayout.Width(100));

            var gameDatas = Resources.LoadAll<GameDataObject>("");
            var names = new List<string>();
            foreach (var item in gameDatas) names.Add(item.name.ToString());

            names.RemoveAll(x => x == "GameData");

            if (names.Count != 0)
            {
                if (types.gameDatas[i].gameData == null)
                {
                    types.gameDatas[i].gameData = gameDatas[0];
                    //Save(types);
                }
                var oldSel = gameDatas.ToList().FindIndex(x => x.name == types.gameDatas[i].gameData.name);
                var selected = EditorGUILayout.Popup("", oldSel, names.ToArray(), GUILayout.MinWidth(80));
                types.gameDatas[i].gameData = gameDatas[selected];

                if (oldSel != selected)
                {
                    //Save(types);
                }
            }
            else
            {
                EditorGUILayout.Popup("", 0, new string[] { "Empty" }, GUILayout.MinWidth(80));
            }

            if (types.gameDatas[i].gameData == null || names.Count == 0)
            {
                GUI.enabled = false;
            }

            var levelList = new List<string>();
            if (GameDataObject.GetMain(true) != null)
            {
                var mn = GameDataObject.GetMain(true);
                for (int j = 0; j < mn.levelList.Count; j++)
                {
                    levelList.Add(mn.levelList[j].name);
                }
            }
            var oldID = types.gameDatas[i].level_id;
            types.gameDatas[i].level_id =  EditorGUILayout.Popup("", types.gameDatas[i].level_id, levelList.ToArray(), GUILayout.MinWidth(50));
            if (oldID != types.gameDatas[i].level_id)
            {
                //Save(types);
            }

            GUI.enabled = true;

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                types.gameDatas.RemoveAt(i);
                //Save(types);
                return;
            }
            GUILayout.EndHorizontal();
        }
    }
}
