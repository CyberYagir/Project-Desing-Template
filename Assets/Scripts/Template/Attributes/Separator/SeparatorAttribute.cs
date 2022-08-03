using UnityEngine;

public class SeparatorAttribute : PropertyAttribute
{
    public float lineHeight, upSpace, downSpace;
    public SeparatorAttribute(float lineHeight = 1, float upSpace = 10, float downSpace = 10)
    {
        this.lineHeight = lineHeight;
        this.upSpace = upSpace;
        this.downSpace = downSpace;
    }
}
