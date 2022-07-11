using System;
using System.Collections;
using System.Collections.Generic;
using Template.Scriptable;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "DataManager", menuName = "Yaroslav/DataManager", order = 1)]
    public class DataManagerObject : ScriptableObject
    {
        [SerializeField] private List<GameDataObject> gameDatas = new List<GameDataObject>();
        [SerializeField] private GameModesObject gameTypes;
        
        public void SetSaveDataForAllGameData()
        {
            for (int i = 1; i < gameDatas.Count; i++)
            {
                gameDatas[i].SetSaves(gameDatas[0].Saves);
            }
        }
        public GameDataObject GetStandardData()
        {
            
            if (gameDatas.Count == 0)
            {
                Debug.LogError("Yaroslav: GameDatas list in DataManager Empty");
                return null;
            }

            return gameDatas[0];
        }


        
        public GameDataObject GetDataByMode()
        {
            if (gameTypes != null)
            {
                return gameTypes.GetGameDataByLevel(this);
            }
            else
            {
                return GetStandardData();
            }
        }


        private static DataManagerObject chache;

        public static GameDataObject StaticGetStandardData() //For Editor
        {
            if (chache == null)
            {
                var datas = Resources.LoadAll<DataManagerObject>("");
                if (datas.Length != 0)
                {
                    chache =  datas[0];
                }
            }

            if (chache != null)
            {
                return chache.GetStandardData();
            }
            else
            {
                return null;
            }
        }

        public void AddGameData(GameDataObject gameData)
        {
            gameDatas.Add(gameData);
        }

        public void SetModesData(GameModesObject modes)
        {
            gameTypes = modes;
        }
    }
}
