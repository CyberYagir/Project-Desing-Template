using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasTemplateText : MonoBehaviour
{
    [TextArea]
    TMP_Text tmp_text;

    private void Start()
    {
        tmp_text = GetComponent<TMP_Text>();
        tmp_text.text += "\n\n<align=center>-----Data-----\n<align=left>\n" +
            "Current GameData: <color=\"orange\">" + GameDataObject.GetData().name + (GameDataObject.GetData().name != "GameData" ? " <color=\"yellow\">(GameType change)</color>" : "") + "</color>\n" +
            "Current Level: <color=\"orange\">" + GameDataObject.GetMain().saves.GetFromPrefs(Prefs.Level) + "</color>\n" +
            "Start By Tap: <color=\"orange\">" + GameDataObject.GetMain().startByTap.ToString() + "</color>";
    }
}
