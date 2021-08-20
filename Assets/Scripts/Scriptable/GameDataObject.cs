using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GeneratorItem
{
    public string name;
    public Color color;
    public GameObject[] prefab;
}
[CreateAssetMenu(fileName = "Game Data", menuName = "Yaroslav/Game Data", order = 1)]
public class GameDataObject : ScriptableObject
{
    public List<GeneratorItem> generatorItems = new List<GeneratorItem>();
    public List<LevelManager> levelManagers = new List<LevelManager>();
    public GameObject playerPrefab, canvas;

    public static GameDataObject GetData()
    {
        return Resources.Load<GameDataObject>("GameData");
    }
}
