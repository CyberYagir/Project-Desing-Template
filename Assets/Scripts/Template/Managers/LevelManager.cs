using UnityEngine;

namespace Template.Managers
{
    /// <summary>
    /// ������ ��� <b>���������</b> ������, � <b>������ �������� ����� �� ������ FindObjectOfType</b>.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public SpawnPoint PlayerSpawn { get; private set; }
        private void Awake(){ PlayerSpawn = GetComponentInChildren<SpawnPoint>();}
    }
}
