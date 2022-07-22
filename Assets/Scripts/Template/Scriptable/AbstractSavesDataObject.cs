using System.Collections.Generic;
using Template.Managers;
using Template.Tweaks;
using UnityEngine;

namespace Template.Scriptable
{
    public abstract class AbstractSavesDataObject : CustomScriptableObject
    {
        [System.Serializable]
        public class OptionsSaveData
        {
            [SerializeField] private bool sound = true;
            [SerializeField] private bool vibration = true;

            public bool Sound => sound;
            public bool Vibration => vibration;


            public void SetSound(bool state)
            {
                sound = state;
            }
            
            public void SetVibration(bool state)
            {
                vibration = state;
            }
        }
        [System.Serializable]
        public class LevelSaveData
        {
            [SerializeField] private int level;
            [SerializeField] private int completedLevels;
            [SerializeField] private int startsCount;

            public int Level => level;
            public int CompletedCount => completedLevels;
            public int StartsCount => startsCount;


            public void SetLevel(int id)
            {
                level = id;
            }
            public void SetCompetedCount(int id)
            {
                completedLevels = id;
            }

            public void AddStartCount()
            {
                startsCount++;
            }

            public void AddLevel()
            {
                level++;
            }

            public void AddCompletedCount()
            {
                completedLevels++;
            }
        }

        [System.Serializable]
        public class PlayerSaveData
        {
            [SerializeField] private int coins;

            public int Coins => coins;
            [System.NonSerialized] 
            public UEvent OnIncreaseMoney = new UEvent();

            public void IncreaseMoney(int addCount, bool trigger = true)
            {
                coins += addCount;
                if (trigger)
                {
                    OnIncreaseMoney.Run();
                }
            }
        }




        [SerializeField] private LevelSaveData levelData = new LevelSaveData();
        [SerializeField] private PlayerSaveData playerData = new PlayerSaveData();
        [SerializeField] private OptionsSaveData optionsData = new OptionsSaveData();

        
        
        public LevelSaveData LevelData => levelData;
        public PlayerSaveData PlayerData => playerData;
        public OptionsSaveData OptionsData => optionsData;
        

        /// <summary>
        /// Задать левел с учётом повторения. Если идекс будет слишком большой то вернёт 0 левел.
        /// </summary>
        /// <param name="id">Номер левела</param>
        public virtual void SetLevel(int id, GameDataObject gameData)
        {
            if (LevelData.CompletedCount == 0)
            {
                LevelData.SetCompetedCount(1);
            }

            if (id >= gameData.Levels.Count)
            {
                LevelData.SetLevel(0);
            }
            else
            {
                LevelData.SetLevel(id);
            }
        }
        public abstract void Save();
        public abstract void Load();
        
    }
}