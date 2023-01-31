using System;
using System.Linq;
using Game.Lightning.Scriptable;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Lightning.Editor
{
    [CustomEditor(typeof(LocalLightningObject))]
    public class LocalLightningObjectEditor : UnityEditor.Editor
    {
        private LocalLightningObject llo;
        private void OnEnable()
        {
            llo = target as LocalLightningObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Save Lightning from " + EditorSceneManager.GetActiveScene().name))
            {
                var lights = FindObjectsOfType<Light>().ToList();
                llo.GetDataFromScene(lights.Find(x => x.type == LightType.Directional));
                llo.SetDirty();
            }
            
            if (GUILayout.Button("Load Lightning from " + EditorSceneManager.GetActiveScene().name))
            {
                var lights = FindObjectsOfType<Light>().ToList();
                llo.LoadDataToScene(lights.Find(x => x.type == LightType.Directional));
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}
