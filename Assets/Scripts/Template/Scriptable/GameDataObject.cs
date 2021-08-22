using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
public class GameDataObject : ScriptableObject
{
    [System.Serializable]
    public class GDOMain //Базовый класс
    {
        public GameObject playerPrefab, canvas;
        public AbstractSavesDataObject saves;
        public bool startByTap;
        public List<LevelManager> levelList = new List<LevelManager>();
    }

    public GDOMain main;

    //Другие переменные


    public static GameDataObject GetData()
    {
        return Resources.Load<GameDataObject>(GameDatasManagerObject.GetGameDataByLevel());
    }
    public static GDOMain GetMain()
    {
        return GetData().main;
    }
}
