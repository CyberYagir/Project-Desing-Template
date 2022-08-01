using System;

namespace Template.Managers
{
    public sealed class LevelEvents
    {
        public Action StartGame { get; set; } //Когда gameStage становится Game
        public Action EndGame { get; set; } //Когда gameStage становится EndWait

        public LevelEvents()
        {
            StartGame = delegate { };
            EndGame = delegate { };
        }

        public void Start()
        {
            StartGame.Invoke();
        }

        public void End()
        {
            EndGame.Invoke();
        }
    }
}