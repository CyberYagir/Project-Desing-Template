using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawTiles : MonoBehaviour
{
    public Color color, glow;
    public LayerMask layerMask;
    private void Update()
    {
        var cast = Physics.SphereCastAll(Vector3Int.RoundToInt(transform.position) - new Vector3(0, 1, 0), 0.25f, Vector3.down, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
        foreach (var item in cast)
        {
            print(item.transform.name);
            if (!item.transform.GetComponent<ChangeColor>())
            {
                item.transform.gameObject.AddComponent<ChangeColor>().Set(color, glow);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3Int.RoundToInt(transform.position) - new Vector3(0, 1, 0), 0.25f);
    }
}
