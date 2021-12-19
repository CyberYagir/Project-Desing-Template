using System.Collections.Generic;
using Template.Scriptable;
using UnityEngine;

[System.Serializable]
public class LevelAndGameData
{
    public int levelID;
    public GameDataObject gameData;
}
[CreateAssetMenu(fileName = "GameTypes", menuName = "Yaroslav/GameTypes", order = 5)]
public class GameDatasManagerObject : CustomScriptableObject
{
    [HideInInspector]
    public List<LevelAndGameData> gameDatas = new List<LevelAndGameData>();
    //public AbstractSavesDataObject saves;
    public static GameDatasManagerObject Instance;
    public static AbstractSavesDataObject SavesData;
    public static bool IsNull = false;

    /// <summary>
    /// Получение текущей GameData в зависимости от типа игры. 
    /// </summary>
    /// <returns>Строка с названием GameDataObject в ресурсах</returns>
    public static string GetGameDataByLevel()
    {
        if (Instance == null && IsNull == false)
        {
            SavesData = GameDataObject.GetData(true).main.saves;
            Instance = Resources.Load<GameDatasManagerObject>("GameTypes");
            if (Instance == null)
            {
                IsNull = true;
            }
        }
        if (Instance == null)
        {
            return "GameData";
        }

        var data = Instance.gameDatas.Find(x => x.levelID == (int)SavesData.GetPref(Prefs.Level));
        if (data != null)
        {
            return data.gameData.name;
        }
        return "GameData";
    }
}
