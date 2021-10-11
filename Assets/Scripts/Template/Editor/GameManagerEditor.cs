using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : EditorTweaks
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("UI: ", GameManager.canvas, typeof(GameObject), true);
            EditorGUILayout.ObjectField("Player: ", GameManager.player, typeof(GameObject), true);
            EditorGUILayout.ObjectField("Level: ", GameManager.currentLevel, typeof(LevelManager), true);
            GUI.enabled = true;
        }
        else
        {
            EditorGUILayout.LabelField("You must be entered into Playmode");
            EditorGUILayout.LabelField("to view static variables. ");
        }
        DrawSeparator();

        GameManager.gameStage = (GameStage)EditorGUILayout.EnumPopup("Game Stage: ", GameManager.gameStage);
    }

    public void SetNull(Object @object)
    {
        if (@object == null)
        {
            @object = null;
        }
    }
}
