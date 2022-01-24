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
        
        public void HorizontalLine ( Color color, float space = 20) {
        
            GUIStyle horizontalLine;
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
            horizontalLine.fixedHeight = 1;
            GUILayout.Space(space/2f);
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box( GUIContent.none, horizontalLine );
            GUI.color = c;
        
            GUILayout.Space(space/2f);
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
