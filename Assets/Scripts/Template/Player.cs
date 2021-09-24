using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пример
/// </summary>
public class Player : MonoBehaviour
{

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.isKinematic = !(GameManger.instance.gameStage == GameStage.Game);
    }
}
