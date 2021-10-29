using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangeAttribute : PropertyAttribute
{
    public string method;
    public object param;

    public OnChangeAttribute(string method, params object[] param)
    {
        this.method = method;
        this.param = param;
    }
}
