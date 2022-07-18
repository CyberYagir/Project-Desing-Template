using System;
using Template.Scriptable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Template.Managers
{
    public enum GameStage { StartWait, Game, EndWait, Pause};
    public class GameManager : SingletonCustom<GameManager>
    {
        [SerializeField] private EventsController eventsController;
        
        public static LevelLogic CurrentLevel { get; private set; }
        public static GameObject Player { get; private set; }
        public static Canvas Canvas { get; private set; }
        public static Camera Camera { get; private set; }

        public static GameStage GameStage { get; set; }
        
        [SerializeField] private DataManagerObject dataManager;

        public static GameDataObject GameData { get; private set; }
        private GameDataObject.GDOMain data => GameData?.MainData;
        public static DataManagerObject DataManager => Instance?.dataManager;
    
        // Эвенты 
        public static event Action StartGame = delegate { }; //Когда gameStage становится Game
        public static event Action EndGame = delegate { }; //Когда gameStage становится EndWait
        public static event Action TapToPlayUI = delegate { }; //Когда игрок тапает в первый раз при data.startByTap

        public float PlayedTime { get; private set; } = 0;



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
            GameData.Saves.LevelData.AddStartCount();
            PlayedTime = Time.time;
            
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
                stdGameData.Saves.LevelData.SetLevel(stdGameData.DebugLevel.levelID);
            }
#endif
            if (stdGameData.Saves == null) { Debug.LogError("Yaroslav: Saves Not Found"); return; }
            if (stdData.levelList == null || stdData.levelList.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + dataManager.GetStandardData().name + "\" is empty"); return; }
            
            stdGameData.Saves.SetLevel(stdGameData.Saves.LevelData.Level);
            CurrentLevel = Instantiate(stdData.levelList[stdGameData.Saves.LevelData.Level]);
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

            data.Saves.LevelData.AddLevel();
            data.Saves.SetLevel(data.Saves.LevelData.Level);
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

        public void SetDataManager(DataManagerObject dataManagerObject)
        {
            dataManager = dataManagerObject;
        }
    }
}