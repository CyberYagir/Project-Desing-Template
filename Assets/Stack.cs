using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public static int stack;
    [SerializeField] Transform backPack;
    private void Start()
    {
        stack = 0;
    }
    private void Update()
    {
        backPack.transform.localScale = Vector3.one * (1 + (stack / 50f));
    }
}
