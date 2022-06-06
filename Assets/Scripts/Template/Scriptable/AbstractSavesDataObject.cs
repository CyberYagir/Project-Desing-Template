using System.Collections.Generic;
using UnityEngine;

namespace Template.Scriptable
{
    
    #region Abstract_Enums 

    /// <summary>
    /// Список всех префов. 
    /// </summary>
    public enum Prefs { Level, CompletedLevels, Points, StartsCount, test} //Добавить элемент для нового префса

    /// <summary>
    /// Возможные типо префов.
    /// </summary>
    public enum PrefType { String, Int, Float, Bool }

    /// <summary>
    /// Класс префа.
    /// </summary>
    [System.Serializable]
    public class PrefsValue
    {
        [ReadOnly]
        public string name;
        public Prefs pref;
        public PrefType savePref;
    }

    #endregion
    public abstract class AbstractSavesDataObject : CustomScriptableObject
    {
        public virtual void SetPref(Prefs prefs, object value)
        {
            
        }

        public virtual void AddToPref(Prefs prefs, object value)
        {
            
        }


        public virtual void SubToPref(Prefs prefs, object value)
        {
            
        }

        public virtual object GetPref(Prefs prefs)
        {
            return null;
        }
        
        /// <summary>
        /// Задать левел с учётом повторения. Если идекс будет слишком большой то вернёт 0 левел.
        /// </summary>
        /// <param name="id">Номер левела</param>
        public virtual void SetLevel(int id)
        {
            if ((int)GetPref(Prefs.CompletedLevels) == 0)
            {
                SetPref(Prefs.CompletedLevels, 1);
            }
            if (id >= GameDataObject.GetMain(true).levelList.Count)
            {
                SetPref(Prefs.Level, 0);
            }
            else
            {
                SetPref(Prefs.Level, id);
            }
        }

        public abstract void Save();
    }
}