

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class TransformEditor : EditorTweaks
{
    Transform obj;
    Vector3 rot;
    bool openOptions;
    bool is2DGame = false;

    void MyUndoCallback()
    {
        rot = obj.localEulerAngles;
    }
    public void OnEnable()
    {
        obj = (Transform)target;
        Undo.undoRedoPerformed += delegate { MyUndoCallback(); };
        if (!EditorPrefs.HasKey("transform[" + obj.GetInstanceID() + "]:rot"))
        {
            rot = obj.localEulerAngles;
        }
        else
        {
            rot = JsonUtility.FromJson<Vector3>(EditorPrefs.GetString(obj.GetInstanceID() + ":rot"));
        }
        openOptions = EditorPrefs.GetInt("trasform:drawOptions", 0) == 1 ? true : false;
        is2DGame = EditorPrefs.GetInt("trasform:is2DGame", 0) == 1 ? true : false;
    }


    public void OnDisable()
    {
        Undo.undoRedoPerformed -= delegate { MyUndoCallback(); };
        EditorPrefs.SetInt("trasform:drawOptions", openOptions ? 1 : 0);
        EditorPrefs.SetInt("trasform:is2DGame", is2DGame ? 1 : 0);
    }
    public const float labelWidth = 65;
    GUILayoutOption[] VetorFieldStyles = new GUILayoutOption[] { GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth) };

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Vector3 newPos = new Vector3();
        Vector3 newScale = new Vector3();
        Vector3 newRot = new Vector3();
        EditorGUI.BeginChangeCheck();
        {
            if (!is2DGame)
            {
                newPos = DrawVectorField("Position:", obj.localPosition);
                newRot = DrawVectorField("Rotation:", rot);
                newScale = DrawVectorField("Scale: ", obj.localScale);
            }
            else
            {
                newPos = DrawVectorField("Position:", obj.localPosition);

                GUILayout.BeginHorizontal();
                {
                    newRot = rot;
                    var oldWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = labelWidth;
                    newRot = new Vector3(newRot.x, newRot.y, EditorGUILayout.FloatField("Rotation: ", newRot.z));
                    EditorGUIUtility.labelWidth = oldWidth;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Scale: ", VetorFieldStyles);
                    newScale = EditorGUILayout.Vector2Field("", (Vector2)obj.localScale);
                    newScale = new Vector3(newScale.x, newScale.y, 1);
                }
                GUILayout.EndHorizontal();
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, $"Change {target.name} transform value");
            obj.localPosition = newPos;
            obj.localScale = newScale;
            obj.localEulerAngles = newRot;
            rot = newRot;
        }
        DrawSeparator();
        if (openOptions)
        {
            EditorGUILayout.BeginVertical();
            {
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Global Values: ");
                }
                EditorGUILayout.EndHorizontal();

                DrawVectorField("Position:", obj.position);
                DrawVectorField("Rotation:", obj.eulerAngles);
                DrawVectorField("Scale: ", obj.localScale);
                GUI.enabled = true;

                
            }

            GUILayout.Space(5);
            is2DGame = EditorGUILayout.Popup("Is 2D Game?: ",is2DGame ? 1 : 0, new string[] { "False", "True" }) == 1 ? true : false;
            if (GUILayout.Button("-")) { openOptions = false; }
            EditorGUILayout.EndVertical();
        }
        else
        {
            if (GUILayout.Button("Options")) { openOptions = true; }
        }
    }

    public Vector3 DrawVectorField(string name, Vector3 input)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(name, VetorFieldStyles);
            input = EditorGUILayout.Vector3Field("", input);
        }
        GUILayout.EndHorizontal();
        return input;
    }
}

