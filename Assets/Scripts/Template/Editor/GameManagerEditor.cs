using Template.Managers;
using UnityEditor;
using UnityEngine;

namespace Template.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : EditorTweaks
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("UI: ", GameManager.Canvas, typeof(GameObject), true);
                EditorGUILayout.ObjectField("Player: ", GameManager.Player, typeof(GameObject), true);
                EditorGUILayout.ObjectField("Level: ", GameManager.CurrentLevel, typeof(LevelManager), true);
                EditorGUILayout.ObjectField("Camera: ", GameManager.Camera, typeof(Camera), true);
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.LabelField("You must be entered into Playmode");
                EditorGUILayout.LabelField("to view static variables. ");
            }
            DrawSeparator();

            GameManager.GameStage = (GameStage)EditorGUILayout.EnumPopup("Game Stage: ", GameManager.GameStage);
        }
    }
}
