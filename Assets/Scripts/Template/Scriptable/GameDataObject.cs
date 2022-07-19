using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
    public class GameDataObject : CustomScriptableObject
    {

        

        [HideInInspector] [SerializeField] private List<LevelLogic> levelList = new List<LevelLogic>();
        [HideInInspector] [SerializeField] private AbstractSavesDataObject saves;
        [HideInInspector] [SerializeField] private SoundDataObject sound;
        
        
        
        public bool isDebugBuild;
        [HideInInspector] public DebugLevel DebugLevel = new DebugLevel();
        
        
        
        public AbstractSavesDataObject Saves => saves;
        public SoundDataObject Sound => sound;
        public List<LevelLogic> Levels => levelList;

        //Остальные переменные

        


        #region Editor
        public void SetData(List<LevelLogic> levels, AbstractSavesDataObject newSaveData) //Editor
        {
            SetLevels(levels);
            SetSaves(newSaveData);
        }

        public void SetLevels(List<LevelLogic> levels)
        {
            levelList = levels;
        }
        public void SetSaves(AbstractSavesDataObject newSaveData) //Editor
        {
            saves = newSaveData;
        }
        public void SetSound(SoundDataObject soundData)
        {
            sound = soundData;
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
        

