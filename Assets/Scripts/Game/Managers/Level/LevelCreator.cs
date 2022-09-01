using UnityEngine;

namespace Template.Managers
{
    [System.Serializable]
    public sealed class LevelCreator
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private UIController canvas;
        [SerializeField] private CameraController camera;

        public PlayerController Player => player;
        public UIController Canvas => canvas;
        public CameraController Camera => camera;

        public void InitAll(LevelLogic logic, GameManager gameManager) 
        {
            Player.Init(logic);
            Canvas.Init(logic, gameManager);
            Camera.Init();
        }
        
    }
}