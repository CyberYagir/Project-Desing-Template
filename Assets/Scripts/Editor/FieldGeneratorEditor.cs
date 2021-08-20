using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldGenerator))]
public class FieldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            FieldGenerator fieldGenerator = (FieldGenerator)target;
            fieldGenerator.Generate();
        }
    }
}
