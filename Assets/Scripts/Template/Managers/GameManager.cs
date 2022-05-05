using System;
using Template.Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Template.Managers
{
    public enum GameStage { StartWait, Game, EndWait };
    /// <summary>
    /// Игровой менеджер.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// <b>Синглетон</b> менеджера для обращения к <b>НЕ</b> статическим методам и переменным. 
        /// </summary>
        public static GameManager Instance { get; private set; }

        //Юзабельное
        /// <summary>
        /// <i>Свойство:</i> <b>LevelManager</b> текущего уровная на сцене.
        /// </summary>
        public static LevelManager CurrentLevel { get; private set; }
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


        public float playedTime = 0;

        #region Mono
        public void Awake()
        {
            Instance = this;

            StartGame = delegate { };
            EndGame = delegate { };
            TapToPlayUI = delegate { };

            GameStage = GameStage.StartWait;

            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);

            if (GameData != GameDataObject.GetData())
            {
                GameData = GameDataObject.GetData();
            }
            data = GameData.main;
            OnLevelStarted(data);
            LoadLevel();
        }
        private void Update()
        {
            if (Instance == null) Instance = this;
            EditorControls();
            TapToStartCheck();
        }

        private void Start()
        {
            if (GameData.main.startByTap)
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
            GameData.main.saves.AddToPref(Prefs.StartsCount, 1);
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
            var stdGameData = GameDataObject.GetData(true);
            var stdData = stdGameData.main;
#if UNITY_EDITOR
            if (stdGameData.DebugLevel.isDebugLevel)
            {
                data.saves.SetPref(Prefs.Level, stdGameData.DebugLevel.levelID);
            }
#endif
            if (stdData.saves == null) { Debug.LogError("Yaroslav: Saves Not Found"); return; }
            if (stdData.levelList == null || stdData.levelList.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + GameDataObject.GetData(true).name + "\" is empty"); return; }
            
            stdData.saves.SetLevel((int)stdData.saves.GetPref(Prefs.Level));
            CurrentLevel = Instantiate(stdData.levelList[(int)stdData.saves.GetPref(Prefs.Level)]);
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

        #region Editor
        /// <summary>
        /// Метод обрабатывает хоткеи во время игры в эдиторе.
        /// </summary>
        public void EditorControls()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLevel();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                data.saves.AddToPref(Prefs.Points, 10);
            }

#endif
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
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Загружает следующий левел (Уровни залуплены).
        /// </summary>
        public static void NextLevel()
        {
            var data = GameDataObject.GetMain(true);

            data.saves.SetPref(Prefs.Level, (int)data.saves.GetPref(Prefs.Level) + 1);

            data.saves.SetLevel((int)data.saves.GetPref(Prefs.Level));
            data.saves.AddToPref(Prefs.CompletedLevels, 1);
            SceneManager.LoadScene(0);
        }

        #endregion
        
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                GameData.main.saves?.Save();
            }
        }

        private void OnApplicationQuit()
        {
            GameData.main.saves?.Save();
        }
    }
}