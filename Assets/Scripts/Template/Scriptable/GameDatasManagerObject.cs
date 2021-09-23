using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelAndGameData
{
    public int level_id;
    public GameDataObject gameData;
}
[CreateAssetMenu(fileName = "GameTypes", menuName = "Yaroslav/GameTypes", order = 5)]
public class GameDatasManagerObject : ScriptableObject
{
    [HideInInspector]
    public List<LevelAndGameData> gameDatas = new List<LevelAndGameData>();
    //public AbstractSavesDataObject saves;
    public static GameDatasManagerObject instance;
    public static AbstractSavesDataObject savesData;
    public static bool isNull = false;
    public static string GetGameDataByLevel()
    {

        if (instance == null && isNull == false)
        {
            savesData = GameDataObject.GetData(true).main.saves;
            instance = Resources.Load<GameDatasManagerObject>("GameTypes");
            if (instance == null)
            {
                isNull = true;
            }
        }
        if (instance == null)
            return "GameData";
        var data = instance.gameDatas.Find(x => x.level_id == (int)savesData.GetPref(Prefs.Level));
        if (data != null)
            return data.gameData.name;

        return "GameData";
    }
}
