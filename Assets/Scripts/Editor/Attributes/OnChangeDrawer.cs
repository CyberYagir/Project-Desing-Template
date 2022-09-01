using System.Collections;
using System.Collections.Generic;
using Template.Attributes;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(OnChangeAttribute))]
public class OnChangeDrawer : PropertyDrawerTweaks
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);
        if (EditorGUI.EndChangeCheck())
        {
            CallMethod(property.serializedObject.targetObject, GetVarValue(attribute, "method").ToString(), GetVarValue(attribute, "param") as object[]);
        }
    }
}
