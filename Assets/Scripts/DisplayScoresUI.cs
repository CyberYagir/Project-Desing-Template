using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoresUI : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void Update()
    {
        text.text = Stack.stack.ToString();
    }
}
