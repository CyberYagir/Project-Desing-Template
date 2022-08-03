using System;
using Template.Scriptable;
using Template.Tweaks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Template.Managers
{
    public enum GamePhase { StartWait, Game, EndWait, Pause};
    public class GameManager : MonoCustom
    {
        [Header("References")]
        [SerializeField] private GameDataObject gameData;
        [SerializeField] private EventsController eventsController;

        [Header("Spawned Data")]
        [ReadOnly] [SerializeField] private LevelLogic CurrentLevel;


        public GameDataObject GameData => gameData;

        private float playedTime = 0;

        #region Mono

        protected override void Awake()
        {
            eventsController.Init();
            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
            if (gameData == null)
            {
                Debug.LogError("Yaroslav: GameData is Empty. Move GameData to GameManager or Click Template>Configurator>Configure Resources/All");
                return;
            }
            gameData.Saves.Load();
            LoadLevel();
            base.Awake();
        }


        public override void OnStart()
        {
            base.OnStart();
            Application.targetFrameRate = 60;
            playedTime = Time.time;
        }
        
        #endregion
        
        
        
        #region Gameplay

        /// <summary>
        /// Этот метод спавнит текущий уровень, игрока и канвас.
        /// </summary>
        public void LoadLevel() //Создание уровня 
        {
            //Игрок и канвас
            SpawnLevel();
            CurrentLevel.Init(this);   
        } 
        public void SpawnLevel()
        {
            var stdGameData = gameData;
#if UNITY_EDITOR
            if (stdGameData.DebugLevel.enableDebug)
            {
                stdGameData.Saves.LevelData.SetLevel(stdGameData.DebugLevel.levelID);
            }
#endif
            if (stdGameData.Saves == null) { Debug.LogError("Yaroslav: Saves Not Found"); return; }
            if (stdGameData.Levels == null || stdGameData.Levels.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + gameData.name + "\" is empty"); return; }
            
            stdGameData.Saves.SetLevel(stdGameData.Saves.LevelData.Level, gameData);
            CurrentLevel = Instantiate(stdGameData.Levels[stdGameData.Saves.LevelData.Level]);

        }
        
        #endregion
        

        #region Static
        
        
        public void OnLevelEnd(bool win = true)
        {
            CurrentLevel.ChangePhase(GamePhase.EndWait);
        
            if (win)
            {
                Debug.Log("Win Event exec");
            }
            else
            {
                Debug.Log("Loose Event exec"); 
            }

            //Эвенты метрик
            //Конец уровня
        }

        /// <summary>
        /// Перезапускает текущий левел. 
        /// </summary>
        public static void Restart(GameDataObject gameData)
        {
            gameData.Saves.Save();
        }

        /// <summary>
        /// Загружает следующий левел (Уровни залуплены).
        /// </summary>
        public static void NextLevel(GameDataObject gameData)
        {
            var data = gameData;

            data.Saves.LevelData.AddLevel();
            data.Saves.SetLevel(data.Saves.LevelData.Level, gameData);
            data.Saves.LevelData.AddCompletedCount();
            data.Saves.Save();
        }

        #endregion
        
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                GameData.Saves?.Save();
            }
        }

        private void OnApplicationQuit()
        {
            GameData.Saves?.Save();
        }
    }
}