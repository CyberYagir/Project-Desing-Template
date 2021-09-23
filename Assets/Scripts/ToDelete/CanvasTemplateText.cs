using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasTemplateText : MonoBehaviour
{
    [TextArea]
    TMP_Text tmp_text;
    [SerializeField] GameDataObject data;
    [SerializeField] GameDataObject.GDOMain main;
    string text;
    private void Start()
    {
        
        data = GameDataObject.GetData();
        main = GameDataObject.GetMain();
        tmp_text = GetComponent<TMP_Text>();

        text = tmp_text.text;
    }
    private void Update()
    {
        tmp_text.text = text + "\n\n<align=center>-----Data-----\n<align=left>\n";
        tmp_text.text += "Current GameData: <color=\"orange\">" + data.name + (data.name != "GameData" ? " <color=\"yellow\">(GameType change)</color>" : "") + "</color>\n";
        tmp_text.text += "Current Level: <color=\"orange\">" + main.saves.GetPref(Prefs.Level) + "</color>\n";
        tmp_text.text += "Current Points: <color=\"orange\">" + main.saves.GetPref(Prefs.Points) + "</color>\n";
        tmp_text.text += "Complited Levels: <color=\"orange\">" + main.saves.GetPref(Prefs.CompletedLevels) + "</color>\n";
        tmp_text.text += "Start By Tap: <color=\"orange\">" + main.startByTap.ToString() + "</color>";
    }
}
