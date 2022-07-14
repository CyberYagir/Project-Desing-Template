using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
    public class GameDataObject : CustomScriptableObject
    {
        [System.Serializable]
        public class GDOMain //Главные данные
        {
            public GameObject playerPrefab, canvas;
            [HideInInspector]public List<LevelLogic> levelList = new List<LevelLogic>();
            public bool isDebugBuild;
            public bool startByTap;
        }


        [HideInInspector] public DebugLevel DebugLevel = new DebugLevel();

        [HideInInspector] [SerializeField] private AbstractSavesDataObject saves;

        [SerializeField] private GDOMain main;
        
        
        public AbstractSavesDataObject Saves => saves;
        public GDOMain MainData => main;

        //Остальные переменные



        public GameDataObject()
        {
            main = new GDOMain();
        }


        #region Editor



        public void SetData(GDOMain newMain, AbstractSavesDataObject newSaveData) //Editor
        {
            main = newMain;
            SetSaves(newSaveData);
        }

        public void SetSaves(AbstractSavesDataObject newSaveData) //Editor
        {
            saves = newSaveData;
        }

        #endregion
    }
}

