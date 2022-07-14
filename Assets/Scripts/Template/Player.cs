using System;
using Template;
using Template.Managers;
using UnityEngine;

public class Player : MonoCustom
{
    private Rigidbody rb;
    

    public override void OnStart()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnUpdate()
    {
        rb.isKinematic = GameManager.GameStage != GameStage.Game;
    }
}
