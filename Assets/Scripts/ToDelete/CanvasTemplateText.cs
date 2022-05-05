using Template.Managers;
using Template.Scriptable;
using TMPro;
using UnityEngine;

public class CanvasTemplateText : MonoBehaviour
{
    [TextArea]
    TMP_Text tmp_text;
    string text;
    private void Start()
    {
        tmp_text = GetComponent<TMP_Text>();

        text = tmp_text.text;
    }
    private void Update()
    {
        var data = GameManager.GameData;
        tmp_text.text = text + "\n\n<align=center>-----Data-----\n<align=left>\n";
        tmp_text.text += "Current GameData: <color=\"orange\">" + data.name + (data.name != "GameData" ? " <color=\"yellow\">(GameType change)</color>" : "") + "</color>\n";
        tmp_text.text += "Current Level: <color=\"orange\">" + data.main.saves.GetPref(Prefs.Level) + "</color>\n";
        tmp_text.text += "Current Points: <color=\"orange\">" + data.main.saves.GetPref(Prefs.Points) + "</color>\n";
        tmp_text.text += "Complited Levels: <color=\"orange\">" + data.main.saves.GetPref(Prefs.CompletedLevels) + "</color>\n";
        tmp_text.text += "Start By Tap: <color=\"orange\">" + data.main.startByTap.ToString() + "</color>";

        if (Input.GetKeyDown(KeyCode.P))
        {
            data.main.saves.AddToPref(Prefs.Points, 10);
        }
    }
}
