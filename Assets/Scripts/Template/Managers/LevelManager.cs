using UnityEngine;

/// <summary>
/// Скрипт для <b>каунтеров</b> левела, и <b>других объектов чтобы не искать FindObjectOfType</b>.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public SpawnPoint playerSpawn { get; private set; }
    private void Awake(){ playerSpawn = GetComponentInChildren<SpawnPoint>();}
}
