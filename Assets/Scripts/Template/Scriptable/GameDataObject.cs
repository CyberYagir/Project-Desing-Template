using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.Scriptable
{

    public struct DebugLevel
    {
        public int LevelID;
        public bool IsDebugLevel;
    }
    [CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
    public class GameDataObject : ScriptableObject
    {
        [System.Serializable]
        public class GDOMain //Главные данные
        {
            public GameObject playerPrefab, canvas;
            [HideInInspector]
            public AbstractSavesDataObject saves;
            public bool startByTap;
            [HideInInspector]
            public List<LevelManager> levelList = new List<LevelManager>();
        }

        public static DebugLevel DebugLevel;
        
        public GDOMain main;

        //Остальные переменные
    
    
    
        public GameDataObject()
        {
            main = new GDOMain();
        }

        public static Dictionary<string, GameDataObject> CachedGameDatas;

        public void Awake()
        {
            CacheGameDatas();
        }

        /// <summary>
        /// Кешированные геймдаты
        /// </summary>
        public static void CacheGameDatas()
        {
            CachedGameDatas = new Dictionary<string, GameDataObject>();
            var alldatas = Resources.LoadAll<GameDataObject>("");
            foreach (var data in alldatas)
            {
                CachedGameDatas.Add(data.name, data);
            }
        }

        /// <summary>
        /// Получение <b>GameData</b> из <b>Resources</b> в Assets
        /// </summary>
        /// <param name="getStandardData">������������ �� GameTypes?</param>
        /// <returns></returns>
        public static GameDataObject GetData(bool getStandardData = false)
        {
            GameDataObject data = null;
            if (CachedGameDatas == null)
            {
                data = Resources.Load<GameDataObject>(getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"); 
                CacheGameDatas();
            }
            else
            {
                try
                {
                    data = CachedGameDatas[getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"];
                }
                catch (System.Exception)
                {
                    CacheGameDatas();
                    data = CachedGameDatas[getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData"];
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
        /// �Получение <b>GDOMain</b> из <b>GameData</b> из <b>Resources</b> в Assets
        /// </summary>
        /// <returns></returns>
        public static GDOMain GetMain(bool getStandardData = false)
        {
            return GetData(getStandardData).main;
        }
    }
}

