using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
public class GameDataObject : ScriptableObject
{
    [System.Serializable]
    public class GDOMain //Базовый класс
    {
        public GameObject playerPrefab, canvas;
        [HideInInspector]
        public AbstractSavesDataObject saves;
        public bool startByTap;
        [HideInInspector]
        public List<LevelManager> levelList = new List<LevelManager>();
    }

    public GDOMain main;

    //Другие переменные
    
    
    
    public GameDataObject()
    {
        main = new GDOMain();
    }

    public static Dictionary<string, GameDataObject> cachedGameDatas;

    public void Awake()
    {
        CacheGameDatas();
    }

    /// <summary>
    /// Кеширование GameDat для того чтобы их не грузить из ресурсов.
    /// </summary>
    public static void CacheGameDatas()
    {
        cachedGameDatas = new Dictionary<string, GameDataObject>();
        var alldatas = Resources.LoadAll<GameDataObject>("");
        foreach (var data in alldatas)
        {
            cachedGameDatas.Add(data.name, data);
        }
    }

    /// <summary>
    /// Получить <b>GameData</b> из папки <b>Resources</b> в Assets
    /// </summary>
    /// <param name="getStandardData">Игнорировать ли GameTypes?</param>
    /// <returns></returns>
    public static GameDataObject GetData(bool getStandardData = false)
    {
        GameDataObject data = null;
        if (cachedGameDatas == null)
        {
            data = Resources.Load<GameDataObject>(getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"); 
            CacheGameDatas();
        }
        else
        {
            try
            {
                data = cachedGameDatas[getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"];
            }
            catch (System.Exception)
            {
                CacheGameDatas();
                data = cachedGameDatas[getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"];
                throw;
            }
        }
        if (data == null) { Debug.LogError("Yaroslav: GameData missing. Go to Menu>Tools>Yaroslav..."); return new GameDataObject(); };
        if (getStandardData == false)
        {
            if (data.main.saves == null)
            {
                data.main.saves = GetData(true).main.saves;
            }
        }

        return data;
    }

    /// <summary>
    /// Получить <b>GDOMain</b> из <b>GameData</b> из папки <b>Resources</b> в Assets
    /// </summary>
    /// <param name="getStandardData">Игнорировать ли GameTypes?</param>
    /// <returns></returns>
    public static GDOMain GetMain(bool getStandardData = false)
    {
        return GetData(getStandardData).main;
    }
}

