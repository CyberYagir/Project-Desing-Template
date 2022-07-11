using UnityEngine;

namespace Template.Managers
{
    /// <summary>
    /// Скрипт для <b>каунтеров</b> левела, и <b>других объектов чтобы не искать FindObjectOfType</b>.
    /// </summary>
    public class LevelManager : MonoCustom
    {
        public SpawnPoint PlayerSpawn { get; private set; }
        private void Awake(){ PlayerSpawn = GetComponentInChildren<SpawnPoint>();}
    }
}
