using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    private SeparatorAttribute attrib;
    public override void OnGUI(Rect position)
    {
        if (!(attribute is SeparatorAttribute))
        {
            return;
        }
        attrib = (attribute as SeparatorAttribute);

        position = EditorGUI.IndentedRect(position);
        position.yMin += attrib.upSpace;
        DrawSeparator(position);
    }
    

    public void DrawSeparator(Rect position)
    {
        EditorGUI.DrawRect(new Rect(position.xMin, position.yMin - (attrib.lineHeight/2f), position.width, attrib.lineHeight), Color.grey);
    }

    public override float GetHeight()
    {
        var attr = (attribute as SeparatorAttribute);
        return attr.upSpace + attr.lineHeight + attr.downSpace;
    }
}
