using System.Collections;
using UnityEngine;
using DrawedDictionary;
using System.Collections.Generic;



[System.Serializable]
public class DrawedDictionaryPreview : MonoBehaviour
{
    public DrawedDictionary.Dictionary<int, string> dictionary = new DrawedDictionary.Dictionary<int, string>();

    [OnChange("Change", 2)]
    public float val;

    public void Change(int arg)
    {
        print("Change method. Args: " + arg);
    }
}
