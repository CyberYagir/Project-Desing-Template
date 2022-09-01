using System.Collections.Generic;
using Template.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Yaroslav/GameData", order = 1)]
    public class GameDataObject : CustomScriptableObject
    {
        [Separator()]
        [SerializeField] private List<string> levelList = new List<string>();
        [SerializeField] private SavesDataObject saves;
        [SerializeField] private SoundDataObject sound;
        
        
        
        [HideInInspector] [SerializeField] private DebugLevel debugLevel;
        [FormerlySerializedAs("isDebugBuild")] [HideInInspector] [SerializeField] private bool isDebugBuildBuild;
        
        
        public SavesDataObject Saves => saves;
        public SoundDataObject Sound => sound;
        public List<string> Levels => levelList;
        public DebugLevel DebugLevel => debugLevel;
        public bool IsDebugBuild
        {
            get => isDebugBuildBuild;
            set => isDebugBuildBuild = value;
        }

        //Остальные переменные

        


        #region Editor
        public void SetData(List<string> levels, SavesDataObject newSaveData) //Editor
        {
            SetLevels(levels);
            SetSaves(newSaveData);
        }

        public void SetLevels(List<string> levels)
        {
            levelList = levels;
        }
        public void SetSaves(SavesDataObject newSaveData) //Editor
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
        

