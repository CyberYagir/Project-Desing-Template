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
    [Header("—писок уровней где GameData отличаетс€ от стандартной(¬носить только их).")]
    [Header("Ќужно дл€ создани€ разных режимов игры на разных левелах.")]
    public List<LevelAndGameData> gameDatas = new List<LevelAndGameData>();
    public AbstractSavesDataObject saves;
    public static string GetGameDataByLevel()
    {
        
        var datas = Resources.Load<GameDatasManagerObject>("GameTypes");
        if (datas == null)
            return "GameData";

        var data = datas.gameDatas.Find(x => x.level_id == (int)datas.saves.GetPref(Prefs.Level));
        if (data != null)
            return data.gameData.name;

        return "GameData";
    }
}
