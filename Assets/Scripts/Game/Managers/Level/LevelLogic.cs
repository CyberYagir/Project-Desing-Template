using Template.Tweaks;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Managers
{
    public class LevelLogic : MonoBehaviour
    {
        private LevelEvents levelEvents;
        
        [Separator()]   
        [SerializeField] private LevelCreator levelCreator;
        [SerializeField] private GamePhase gamePhase;
        private GameManager gameManager;
        public GamePhase GamePhase => gamePhase;
        public UnityEvent<GamePhase> OnChangePhase { get; set; } = new UnityEvent<GamePhase>();
        



        public void ChangePhase(GamePhase phase)
        {
            gamePhase = phase;
            OnChangePhase?.Invoke(gamePhase);
        }

        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            ChangePhase(GamePhase.Game);
            levelEvents = new LevelEvents();
            levelCreator.InitAll(this, gameManager); 
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
