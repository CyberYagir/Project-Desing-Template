using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class LevelManager : MonoBehaviour
{
    public Transform playerSpawn;
    public Transform tilesHolder;

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (playerSpawn != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3Int.RoundToInt(playerSpawn.position), Vector3.one);
        }
    }
}
