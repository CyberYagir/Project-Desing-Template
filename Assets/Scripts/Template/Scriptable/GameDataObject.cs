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

    public GameDataObject()
    {
        main = new GDOMain();
    }

    public static GameDataObject GetData()
    {
        var data = Resources.Load<GameDataObject>(GameDatasManagerObject.GetGameDataByLevel());

        if (data == null) { Debug.LogError("Yaroslav: GameData missing. Go to Menu>Tools>Yaroslav..."); return new GameDataObject(); };
        return data;
    }
    public static GDOMain GetMain()
    {
        return GetData().main;
    }
}
