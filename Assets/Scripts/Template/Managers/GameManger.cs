using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStage { StartWait, Game, EndWait };
public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    
    //Юзабельное
    [ReadOnly] [SerializeField] LevelManager currentLevel;
    [ReadOnly] [SerializeField] GameObject player;
    [ReadOnly] [SerializeField] Canvas canvas;
    [ReadOnly] public GameStage gameStage;
    
    //Данные
    GameDataObject.GDOMain data;



    /// Эвенты 
    public static event System.Action StartGame = delegate { }; //Когда gameStage становится Game
    public static event System.Action EndGame = delegate { }; //Когда gameStage становится EndWait

    public static event System.Action TapToPlayUI = delegate { }; //Когда игрок тапает в первый раз при data.startByTap
    
    public static event System.Action LevelWin = delegate { }; //Когда победил
    public static event System.Action LevelLoose = delegate { }; //Когда проиграл




    #region Mono
    public void Awake()
    {
        StartGame = delegate { };
        EndGame = delegate { };
        TapToPlayUI = delegate { };
        LevelWin = delegate { };
        LevelLoose = delegate { };
    }
    private void Start()
    {
        instance = this;
        data = GameDataObject.GetMain();
        OnLevelStarted(data);
        LoadLevel();
        
    }
    private void Update()
    {
        EditorControls();
        TapToStartCheck();
    }

    #endregion

    #region Gameplay
    public void TapToStartCheck() //Проверка на вервый тап 
    {
        if (data.startByTap)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (gameStage == GameStage.StartWait)
                {
                    StarGameByTap();
                    gameStage = GameStage.Game;
                    TapToPlayUI();
                    StartGame();
                }
            }
        }
    }
    public void LoadLevel() //Создание уровня 
    {
        if (data.saves == null){ Debug.LogError("Yaroslav: Saves Not Found"); return; }

        data.saves.SetLevel((int)data.saves.GetFromPrefs(Prefs.Level));
        currentLevel = Instantiate(data.levelList[(int)data.saves.GetFromPrefs(Prefs.Level)]);
        //Игрок и канвас
        if (data.playerPrefab)
        {
            Vector3 spawnPoint = Vector3.zero;
            if (currentLevel.playerSpawn != null)
            {
                spawnPoint = currentLevel.playerSpawn.position; // Затычка чтобы заспавнить Темлейт
                //Настройка игрока
            }
            else
            {
                Debug.LogError("Yaroslav: Spawn Not Found");
            }
            player = Instantiate(data.playerPrefab, spawnPoint, Quaternion.identity);
        }
        if (data.canvas)
            canvas = Instantiate(data.canvas.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Canvas>();
    } 
    public void StopGamePlay() //Остановка игры (Игрока и тд.) 
    {
        //Выключение игрока и др.
    }
    public void StarGameByTap() //Включение при тапе (Игрока и тд.)
    {
        //Включение управления и др.
    }

    #endregion

    #region Editor
    public void EditorControls()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
        #endif
    }
    #endregion

        #region Static
    public static void OnLevelStarted(GameDataObject.GDOMain data)
    {
        if (data.startByTap)
        {
            instance.gameStage = GameStage.StartWait;
            instance.StopGamePlay();
        }
        else
        {
            instance.gameStage = GameStage.Game;
            StartGame();
        }

        //Старт урованя 
    }
    public static void OnLevelEnd(bool win = true)
    {
        instance.StopGamePlay();
        if (win) LevelWin(); else LevelLoose();
        instance.gameStage = GameStage.EndWait;
        EndGame();
        //Конец уровня
    }
    public static void Restart()
    {
        OnLevelEnd();
        SceneManager.LoadScene(0);
    }
    public static void NextLevel()
    {
        OnLevelEnd();
        var data = GameDataObject.GetMain();
        data.saves.SetValue(Prefs.Level, (int)data.saves.GetFromPrefs(Prefs.Level) + 1);
        data.saves.SetLevel((int)data.saves.GetFromPrefs(Prefs.Level));
        SceneManager.LoadScene(0);
    }
    #endregion
}
