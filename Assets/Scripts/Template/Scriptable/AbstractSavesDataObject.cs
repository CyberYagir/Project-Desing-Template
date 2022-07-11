using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.Scriptable
{
    public abstract class AbstractSavesDataObject : CustomScriptableObject
    {
        public int level = 5;
        public int completedLevels;
        public int startsCount;
        public int points;


        /// <summary>
        /// Задать левел с учётом повторения. Если идекс будет слишком большой то вернёт 0 левел.
        /// </summary>
        /// <param name="id">Номер левела</param>
        public virtual void SetLevel(int id)
        {
            if (completedLevels == 0)
            {
                completedLevels = 1;
            }

            if (id >= GameManager.Instance.dataManager.GetStandardData().MainData.levelList.Count)
            {
                level = 0;
            }
            else
            {
                level = id;
            }
        }

        public abstract void Save();
        public abstract void Load();
        
    }
}