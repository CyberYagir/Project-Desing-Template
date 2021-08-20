using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField] LevelManager currentLevel;
    [SerializeField] GameObject player;
    [SerializeField] Canvas canvas;
    GameDataObject.GDOMain data;



    private void Start()
    {
        data = GameDataObject.GetMain();
        OnLevelStarted();
        LoadLevel();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
    }

#endif

    public static void OnLevelStarted()
    {
        //Старт урованя 
    }
    public static void OnLevelEnd()
    {
        //Конец уровня
    }
    public void LoadLevel()
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
}
