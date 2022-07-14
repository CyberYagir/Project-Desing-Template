using System;
using Template.Scriptable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Event = Template.Tweaks.Event;

namespace Template.Managers
{
    public enum GameStage { StartWait, Game, EndWait, Pause};
    /// <summary>
    /// Игровой менеджер.
    /// </summary>
    public class GameManager : SingletonCustom<GameManager>
    {
        [SerializeField] private EventsController eventsController;

        //Юзабельное
        /// <summary>
        /// <i>Свойство:</i> <b>LevelManager</b> текущего уровная на сцене.
        /// </summary>
        public static LevelLogic CurrentLevel { get; private set; }
        /// <summary>
        /// <i>Свойство:</i> Текущий игрок на сцене.
        /// </summary>
        public static GameObject Player { get; private set; }
        /// <summary>
        /// <i>Свойство:</i> <b>Canvas</b> текущего уровная на сцене.
        /// </summary>
        public static Canvas Canvas { get; private set; }
        
        public static Camera Camera { get; private set; }
        /// <summary>
        /// <i>Свойство:</i> Стадия игры на которой сейчас находится игрок. 
        /// </summary>
        public static GameStage GameStage;
        
        public DataManagerObject dataManager;
        
        //Данные
        GameDataObject.GDOMain data;
        public static GameDataObject GameData;
    
        // Эвенты 
        /// <summary>
        /// <i>Эвент</i> Вызывается при <b>GameStage</b> равному <b>Game</b>
        /// </summary>
        public static event System.Action StartGame = delegate { }; //Когда gameStage становится Game
        /// <summary>
        /// <i>Эвент</i> Вызывается при <b>GameStage</b> равному <b>EndWait</b>
        /// </summary>
        public static event System.Action EndGame = delegate { }; //Когда gameStage становится EndWait
        /// <summary>
        /// <i>Эвент</i> Вызывается при первом нажатии на экран, если при этом свойство <b>startByTab</b> в <b>GameData</b> равно <b>True</b>
        /// </summary>
        public static event System.Action TapToPlayUI = delegate { }; //Когда игрок тапает в первый раз при data.startByTap

        [HideInInspector]
        public float playedTime = 0;


        
        #region Mono

        protected override void Awake()
        {
            SingletonSet(this);
            eventsController.Init();
            
            StartGame = delegate { };
            EndGame = delegate { };
            TapToPlayUI = delegate { };

            GameStage = GameStage.StartWait;

            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);


            if (dataManager == null)
            {
                Debug.LogError("Yaroslav: DataManager is Empty. Move DataManager to GameManager or Click Template>Configurator>Configure Resources/All");
                return;
            }

            dataManager.SetSaveDataForAllGameData();
            GameData = dataManager.GetDataByMode();
            GameData.Saves.Load();
            data = GameData.MainData;
            OnLevelStarted(data);
            LoadLevel();
            
            base.Awake();
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Instance == null)
                SingletonSet(this);
            TapToStartCheck();
        }


        public override void OnStart()
        {
            base.OnStart();
            Application.targetFrameRate = 60;
            if (GameData.MainData.startByTap)
            {
                StartGame += StartCache;
            }
            else
            {
                StartCache();
            }
        }
        
        #endregion

        public void StartCache()
        {
            Debug.Log("Start event Exec");
            GameData.Saves.startsCount += 1;
            playedTime = Time.time;
            
        }
        
        
        #region Gameplay

        /// <summary>
        /// Если свойство <b>startByTab</b> в <b>GameData</b> равно <b>True</b> то вызывается этот метод. Он проверяет первый тап. После стартует TapToPlayUI().
        /// </summary>
        public void TapToStartCheck() //Проверка на вервый тап 
        {
            if (data.startByTap)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (GameStage == GameStage.StartWait)
                    {
                        StartGameByTap();
                        GameStage = GameStage.Game;
                        TapToPlayUI();
                        StartGame();
                    }
                }
            }
        }

        /// <summary>
        /// Этот метод спавнит текущий уровень, игрока и канвас.
        /// </summary>
        public void LoadLevel() //Создание уровня 
        {
            var stdGameData = dataManager.GetStandardData();
            var stdData = stdGameData.MainData;
#if UNITY_EDITOR
            if (stdGameData.DebugLevel.isDebugLevel)
            {
                stdGameData.Saves.level = stdGameData.DebugLevel.levelID;
            }
#endif
            if (stdGameData.Saves == null) { Debug.LogError("Yaroslav: Saves Not Found"); return; }
            if (stdData.levelList == null || stdData.levelList.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + dataManager.GetStandardData().name + "\" is empty"); return; }
            
            stdGameData.Saves.SetLevel(stdGameData.Saves.level);
            CurrentLevel = Instantiate(stdData.levelList[stdGameData.Saves.level]);
            //Игрок и канвас
            SpawnPlayer();
            SpawnCanvas();
            FindCamera();
        }

        /// <summary>
        /// Этот метод спавнит игрока.
        /// </summary>
        public void SpawnPlayer()
        {
            if (data.playerPrefab)
            {
                Vector3 spawnPoint = Vector3.zero;
                if (CurrentLevel.PlayerSpawn != null)
                {
                    spawnPoint = CurrentLevel.PlayerSpawn.transform.position;
                    //Настройка игрока
                }
                else
                {
                    Debug.LogError("Yaroslav: Spawn Not Found");
                }
                Player = Instantiate(data.playerPrefab, spawnPoint, Quaternion.identity);
            }
        }
        /// <summary>
        /// Этот метод нахождения камеры.
        /// </summary>
        public void FindCamera()
        {
            Camera = Camera.main;
        }
        
        /// <summary>
        /// Этот метод спавнит канвас.
        /// </summary>
        public void SpawnCanvas()
        {
            if (data.canvas)
                Canvas = Instantiate(data.canvas.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Canvas>();
        }

        /// <summary>
        /// Метод для действий во время остановки игры.
        /// </summary>
        public void StopGamePlay() //Остановка игры (Игрока и тд.) 
        {
            //Выключение игрока и др.
        }

        /// <summary>
        /// Метод для действий во время тапа.
        /// </summary>
        public void StartGameByTap() //Включение при тапе (Игрока и тд.)
        {
            //Включение управления и др.
        }

        #endregion
        

        #region Static
        /// <summary>
        /// Задаёт <b>GameStage</b> и вызывает методы начала и стопа игры в зависимости от <b>GameStage</b> (Вызывает эвенты).
        /// </summary>
        /// <param name="data">GameData текущего уровня</param>
        public static void OnLevelStarted(GameDataObject.GDOMain data)
        {
            if (data.startByTap)
            {
                GameStage = GameStage.StartWait;
                Instance.StopGamePlay();
            }
            else
            {
                GameStage = GameStage.Game;
                StartGame();
            }
        }

        /// <summary>
        /// Заканчивает уровни в вызывает эвенты. (Нужен для метрик)
        /// </summary>
        /// <param name="win">Булевая перменная победы</param>
        public static void OnLevelEnd(bool win = true)
        {
            Instance.StopGamePlay();
            GameStage = GameStage.EndWait;
            EndGame();
        
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
        public static void Restart()
        {
            //SceneManager.LoadScene(0);
            GameData.Saves.Save();
        }

        /// <summary>
        /// Загружает следующий левел (Уровни залуплены).
        /// </summary>
        public static void NextLevel()
        {
            var data = Instance.dataManager.GetStandardData();

            data.Saves.level++;
            data.Saves.SetLevel(data.Saves.level);
            data.Saves.completedLevels++;
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