using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStack : MonoBehaviour
{

    private void Start()
    {
        transform.position = Vector3Int.CeilToInt(transform.position);
    }
    private void Update()
    {
        if (!GetComponent<Collider>())
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 10 * Time.deltaTime);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Stack.stack++;
        }
        Destroy(gameObject, 3);
        Destroy(GetComponent<Collider>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3Int.RoundToInt(transform.position), Vector3.one);
    }
}
