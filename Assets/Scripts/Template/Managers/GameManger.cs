using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField] LevelManager currentLevel;
    [SerializeField] GameObject player;
    [SerializeField] Canvas canvas;
    private void Start()
    {
        OnLevelStarted();
        LoadLevel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
    }

    public static void OnLevelStarted()
    {
        //Metriki
    }
    public static void OnLevelEnd()
    {
        //Metriki
    }
    public void LoadLevel()
    {
        currentLevel = Instantiate(GameDataObject.GetData().levelManagers[(int)SavesDataObject.GetFromPrefs(SavesDataObject.Prefs.Level)]);
        //Игрок и канвас
        if (GameDataObject.GetData().playerPrefab)
        {
            player = Instantiate(GameDataObject.GetData().playerPrefab, currentLevel.playerSpawn.position, Quaternion.identity);
            if (currentLevel.playerSpawn != null)
            {
                player.GetComponent<Player>().SetPoint(Vector3Int.RoundToInt(currentLevel.playerSpawn.position));
            }
            else
            {
                Debug.LogError("Spawn Not Found;");
            }
        }
        if (GameDataObject.GetData().canvas)
            canvas = Instantiate(GameDataObject.GetData().canvas.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Canvas>();
    }
    public static void Restart()
    {
        OnLevelEnd();
        SceneManager.LoadScene(0);
    }
    public static void NextLevel()
    {
        OnLevelEnd();
        SavesDataObject.SetValue(SavesDataObject.Prefs.Level, (int)SavesDataObject.GetFromPrefs(SavesDataObject.Prefs.Level) + 1);
        SavesDataObject.SetLevel((int)SavesDataObject.GetFromPrefs(SavesDataObject.Prefs.Level));
        SceneManager.LoadScene(0);
    }
}
