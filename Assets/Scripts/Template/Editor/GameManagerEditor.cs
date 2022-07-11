using Template.Managers;
using Template.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Template.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : EditorTweaks
    {
        public override void OnInspectorGUI()
        {
            var gameManager = target as GameManager;
            gameManager.dataManager = (DataManagerObject) EditorGUILayout.ObjectField("Data Manager: ", gameManager.dataManager, typeof(DataManagerObject), allowSceneObjects: false);
            DrawSeparator();
            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("UI: ", GameManager.Canvas, typeof(GameObject), true);
                EditorGUILayout.ObjectField("Player: ", GameManager.Player, typeof(GameObject), true);
                EditorGUILayout.ObjectField("Level: ", GameManager.CurrentLevel, typeof(LevelManager), true);
                EditorGUILayout.ObjectField("Camera: ", GameManager.Camera, typeof(Camera), true);
                EditorGUILayout.ObjectField("GameData: ", GameManager.GameData, typeof(GameDataObject), true);
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
