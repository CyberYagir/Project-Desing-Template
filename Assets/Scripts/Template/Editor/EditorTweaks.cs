using UnityEditor;
using UnityEngine;

namespace Template.Editor
{
    public abstract class EditorTweaks : UnityEditor.Editor
    {
        public void Save(Object obj)
        {
            SaveObject(obj);
        }
        public void DrawSeparator()
        {
            GUILayout.Space(10);
            DrawLine();
            GUILayout.Space(10);
        }
        public void DrawLine(int h = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, h);
            rect.height = h;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
        
        public static void SaveObject(Object obj)
        {
            Debug.Log(obj.name + " Saved!");
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
