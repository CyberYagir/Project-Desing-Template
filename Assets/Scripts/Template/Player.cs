using UnityEngine;

/// <summary>
/// ����������� ������ ������
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
        rb.isKinematic = !(GameManager.gameStage == GameStage.Game);
    }
}
