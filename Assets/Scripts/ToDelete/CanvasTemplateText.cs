using Template;
using Template.Managers;
using Template.Scriptable;
using TMPro;
using UnityEngine;

public class CanvasTemplateText : MonoCustom
{
    [TextArea]
    TMP_Text tmp_text;
    string text;
    private GameDataObject data = null;

    public override void OnStart()
    {
        base.OnStart();
        tmp_text = GetComponent<TMP_Text>();
        data = GameManager.Instance.dataManager.GetDataByMode();
        text = tmp_text.text;
        UpdateText();
    }

    public void UpdateText()
    {
        tmp_text.text = text + "\n\n<align=center>-----Data-----\n<align=left>\n";
        tmp_text.text += "Current GameData: <color=\"orange\">" + data.name + (data.name != "GameData" ? " <color=\"yellow\">(GameType change)</color>" : "") + "</color>\n";
        tmp_text.text += "Current Level: <color=\"orange\">" + data.Saves.level + "</color>\n";
        tmp_text.text += "Current Points: <color=\"orange\">" + data.Saves.points + "</color>\n";
        tmp_text.text += "Completed Levels: <color=\"orange\">" + data.Saves.completedLevels + "</color>\n";
        tmp_text.text += "Start By Tap: <color=\"orange\">" + data.MainData.startByTap.ToString() + "</color>";
    }

    public override void OnUpdate()
    {
        base.OnUpdate();


        if (Input.GetKeyDown(KeyCode.P))
        {
            data.Saves.points += 10;
            UpdateText();
        }

    }
}
