using Template.Tweaks;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Managers
{
    public class LevelLogic : MonoCustom
    {
        [SerializeField] private LevelData levelData;
        
        private LevelCreator levelCreator;
        private LevelEvents levelEvents;
        
        
        [Space] [SerializeField] private SpawnPoint playerSpawn;
        [SerializeField] private GamePhase gamePhase;
        private GameManager gameManager;
        public SpawnPoint PlayerSpawn => playerSpawn;
        public GamePhase GamePhase => gamePhase;
        public UnityEvent<GamePhase> OnChangePhase { get; set; } = new UnityEvent<GamePhase>();


        public virtual void ConfigurePlayer()
        {
            if (PlayerSpawn == null)
            {
                playerSpawn = GetComponentInChildren<SpawnPoint>();
            }

            if (PlayerSpawn != null)
            {
                levelCreator.Player.transform.position = PlayerSpawn.transform.position;
                //Настройка игрока
            }
        }



        public void ChangePhase(GamePhase phase)
        {
            gamePhase = phase;
            OnChangePhase?.Invoke(gamePhase);
        }

        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            ChangePhase(GamePhase.Game);
            levelCreator = new LevelCreator(levelData, this, gameManager);
            levelEvents = new LevelEvents();
            ConfigurePlayer();
        }

        public void StartGame()
        {
            levelEvents.Start();
        }
        
        public void EndGame()
        {
            levelEvents.End();
        }
    }
}
