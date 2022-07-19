using System;

namespace Template.Managers
{
    public sealed class LevelEvents
    {
        public Action StartGame { get; set; } //Когда gameStage становится Game
        public Action EndGame { get; set; } //Когда gameStage становится EndWait
        public Action TapToPlayUI { get; set; } //Когда игрок тапает в первый раз при data.startByTap


        public LevelEvents()
        {
            StartGame = delegate { };
            EndGame = delegate { };
            TapToPlayUI = delegate { };
        }
    }
}