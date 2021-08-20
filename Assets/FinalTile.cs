using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTile : MonoBehaviour
{

    public bool isIn;
    Vector3 cameraPos;
    private void Start()
    {
        cameraPos = Camera.main.transform.position;
    }

    private void Update()
    {
        if (isIn)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos + new Vector3(0, 15, 0), 10 * Time.deltaTime);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().enabled = false;
            isIn = true;
            FindObjectOfType<Boss>().enabled = true;
        }
    }
}
