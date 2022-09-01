using Template.Managers;
using UnityEngine;

namespace Template
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody rb;
        
        public void Init(LevelLogic level)
        {
            rb = GetComponent<Rigidbody>();
            level.OnChangePhase.AddListener(OnChangePhase);
            OnChangePhase(level.GamePhase);
        }


        public void OnChangePhase(GamePhase phase)
        {
            rb.isKinematic = phase != GamePhase.Game;
        }
    }
}
