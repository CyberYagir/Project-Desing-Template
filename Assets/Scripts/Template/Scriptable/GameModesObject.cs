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
public class GameModesObject : CustomScriptableObject
{
    [HideInInspector]
    public List<LevelAndGameData> gameDatas = new List<LevelAndGameData>();
    public static bool IsNull = false;

    /// <summary>
    /// Получение текущей GameData в зависимости от типа игры. 
    /// </summary>
    /// <returns>Строка с названием GameDataObject в ресурсах</returns>
    public GameDataObject GetGameDataByLevel(DataManagerObject manager)
    {
        var gameData = manager.GetStandardData();
        var saves = gameData.Saves;

        var data = gameDatas.Find(x => x.levelID == saves.LevelData.Level);
        if (data != null)
        {
            return data.gameData;
        }

        return gameData;
    }
}
