using UnityEngine;

namespace Template.Managers
{
    public sealed class LevelCreator
    {
        LevelData levelData;

        public PlayerController Player { get; private set; }
        public UIController Canvas { get; private set; }
        public CameraController Camera { get; private set; }

        public LevelCreator(LevelData data, LevelLogic logic, GameManager gameManager)
        {
            levelData = data;
            SpawnPlayer();
            SpawnCamera();
            SpawnUI();
            InitAll(logic, gameManager);
        }

        private void InitAll(LevelLogic logic, GameManager gameManager)
        {
            Player.Init(logic);
            Canvas.Init(logic, gameManager);
            Camera.Init();
        }

        
        private void SpawnCamera()
        {
            if (levelData.cameraController != null)
            {
                Camera = Object.Instantiate(levelData.cameraController);
            }
        }

        private void SpawnPlayer()
        {
            if (levelData.playerController)
            {
                Player = Object.Instantiate(levelData.playerController);
            }
        }

        private void SpawnUI()
        {
            if (levelData.canvasController)
            {
                Canvas = Object.Instantiate(levelData.canvasController);
            }
        }
    }
}