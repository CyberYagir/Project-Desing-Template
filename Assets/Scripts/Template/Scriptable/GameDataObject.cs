using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
public class GameDataObject : ScriptableObject
{
    [System.Serializable]
    public class GDOMain //������� �����
    {
        public GameObject playerPrefab, canvas;
        [HideInInspector]
        public AbstractSavesDataObject saves;
        public bool startByTap;
        [HideInInspector]
        public List<LevelManager> levelList = new List<LevelManager>();
    }

    public GDOMain main;

    //������ ����������

    public GameDataObject()
    {
        main = new GDOMain();
    }

    /// <summary>
    /// �������� <b>GameData</b> �� ����� <b>Resources</b> � Assets
    /// </summary>
    /// <param name="getStandardData">������������ �� GameTypes?</param>
    /// <returns></returns>
    public static GameDataObject GetData(bool getStandardData = false)
    {
        var data = Resources.Load<GameDataObject>(getStandardData == false ? GameDatasManagerObject.GetGameDataByLevel() : "GameData");
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
    /// �������� <b>GDOMain</b> �� <b>GameData</b> �� ����� <b>Resources</b> � Assets
    /// </summary>
    /// <param name="getStandardData">������������ �� GameTypes?</param>
    /// <returns></returns>
    public static GDOMain GetMain(bool getStandardData = false)
    {
        return GetData(getStandardData).main;
    }
}
