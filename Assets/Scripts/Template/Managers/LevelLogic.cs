using UnityEngine;

namespace Template.Managers
{
    public class LevelLogic : MonoCustom { 
        public SpawnPoint PlayerSpawn { get; protected set; }
        
        protected virtual void Awake(){ PlayerSpawn = GetComponentInChildren<SpawnPoint>();}
    }
}
