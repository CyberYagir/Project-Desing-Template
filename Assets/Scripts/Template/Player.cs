using Template.Managers;
using UnityEngine;

/// <summary>
/// Стандартный скрипт игрока
/// </summary>
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.isKinematic = GameManager.GameStage != GameStage.Game;
    }
}
