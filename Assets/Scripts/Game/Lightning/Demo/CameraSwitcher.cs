using System;
using System.Collections;
using System.Collections.Generic;
using Game.Lightning;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CameraFogger[] foggers;

    private int index;

    private void Start()
    {
        for (int i = 0; i < foggers.Length; i++)
        {
            foggers[i].Init();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            ChangeCamera();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        if (index >= foggers.Length)
        {
            index = 0;
        }else
        if (index < 0)
        {
            index = foggers.Length - 1;
        }
        
        foggers[index].ActivateCamera();
    }
}
