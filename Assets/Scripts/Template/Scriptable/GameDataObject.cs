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
            [HideInInspector]public List<LevelLogic> levelList = new List<LevelLogic>();
            public bool isDebugBuild;
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


#if UNITY_EDITOR
        private static GameDataObject chache;
        public static GameDataObject StaticGetStandardData() //For Editor
        {
            if (chache == null)
            {
                var datas = Resources.LoadAll<GameDataObject>("");
                if (datas.Length != 0)
                {
                    chache = datas[0];
                }
            }

            return chache;
        }
#endif
    }
}
        

