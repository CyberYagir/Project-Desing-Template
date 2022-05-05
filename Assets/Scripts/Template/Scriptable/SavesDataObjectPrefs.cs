using System;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "SavesData", menuName = "Yaroslav/SavesData Prefs", order = 2)]
    public class SavesDataObjectPrefs : AbstractSavesDataObject
    {
                /// <summary>
        /// Активные префы.
        /// </summary>
        [HideInInspector]
        public List<PrefsValue> prefsValues = new List<PrefsValue>();



        /// <summary>
        /// Получить преф из PlayerPrefs.
        /// </summary>
        /// <param name="prefs">Имя префа</param>
        /// <returns>Данные префа</returns>
        public override object GetPref(Prefs prefs)
        {
            var find = FindPrefValue(prefs);
            if (find != null)
            {
                var prefType = find.savePref;
                if (PlayerPrefs.HasKey(prefs.ToString()))
                {
                    switch (prefType)
                    {
                        case PrefType.String:
                            return PlayerPrefs.GetString(prefs.ToString());
                        case PrefType.Int:
                            return PlayerPrefs.GetInt(prefs.ToString());
                        case PrefType.Float:
                            return PlayerPrefs.GetFloat(prefs.ToString());
                        case PrefType.Bool:
                            return (PlayerPrefs.GetInt(prefs.ToString()) == 1 ? true : false);
                        default:
                            return 0;
                    }
                }
                switch (prefType)
                {
                    case PrefType.String:
                        return "NULL";
                    case PrefType.Int:
                        return 0;
                    case PrefType.Float:
                        return 0f;
                    case PrefType.Bool:
                        return false;
                }
            }
            else PrefNotFoundError(prefs);
            return 0;
        }

        /// <summary>
        /// Задать преф из PlayerPrefs.
        /// </summary>
        /// <param name="prefs">Имя префа</param>
        /// <param name="value">Данные префа (String, Int, Float, Bool)</param>
        public override void SetPref(Prefs prefs, object value)
        {
            var find = FindPrefValue(prefs);
            if (find != null)
            {
                var prefType = find.savePref;
                switch (prefType)
                {
                    case PrefType.String:
                        PlayerPrefs.SetString(prefs.ToString(), value.ToString());
                        break;
                    case PrefType.Int:
                        PlayerPrefs.SetInt(prefs.ToString(), (int)value);
                        break;
                    case PrefType.Float:
                        PlayerPrefs.SetFloat(prefs.ToString(), (float)value);
                        break;
                    case PrefType.Bool:
                        PlayerPrefs.SetInt(prefs.ToString(), ((bool)value) == true ? 1 : 0);
                        break;
                    default:
                        Debug.LogError("Yaroslav: Save Data: Save Case Error; Type not found");
                        break;
                }
            }
            else PrefNotFoundError(prefs);
        }

        /// <summary>
        ///  Добавить данные в выбраный преф.
        /// </summary>
        /// <param name="prefs">Имя префа</param>
        /// <param name="value">Обьект который надо прибавить</param>
        public override void AddToPref(Prefs prefs, object value)
        {
            var find = FindPrefValue(prefs);
            if (find != null)
            {
                var prefType = prefsValues.Find(x => x.pref == prefs).savePref;
                switch (prefType)
                {
                    case PrefType.String:
                        SetPref(prefs, (string)GetPref(prefs) + value.ToString());
                        break;
                    case PrefType.Int:
                        SetPref(prefs, (int)GetPref(prefs) + (int)value);
                        break;
                    case PrefType.Float:
                        SetPref(prefs, (float)GetPref(prefs) + (float)value);
                        break;
                    case PrefType.Bool:
                        Debug.LogError("Yaroslav: Save Data: Add Error; Can`t add to Bool");
                        break;
                    default:
                        Debug.LogError("Yaroslav: Save Data: Save Case Error; Type not found");
                        break;
                }
            }
            else PrefNotFoundError(prefs);
        }

        /// <summary>
        /// Отнимает данные от выбранного префа.
        /// </summary>
        /// <param name="prefs">Имя префа</param>
        /// <param name="value">Обьект который надо отнять</param>
        public override void SubToPref(Prefs prefs, object value)
        {
            var prefType = prefsValues.Find(x => x.pref == prefs).savePref;
            switch (prefType)
            {
                case PrefType.String:
                    SetPref(prefs, GetPref(prefs).ToString().Replace((string)value, ""));
                    break;
                case PrefType.Int:
                    SetPref(prefs, (int)GetPref(prefs) - (int)value);
                    break;
                case PrefType.Float:
                    SetPref(prefs, (float)GetPref(prefs) - (float)value);
                    break;
                case PrefType.Bool:
                    Debug.LogError("Yaroslav: Save Data: Substract Error; Can`t add to Bool");
                    break;
                default:
                    Debug.LogError("Yaroslav: Save Data: Save Case Error; Type not found");
                    break;
            }
        }


        /// <summary>
        /// Найти преф в списке активных префов.
        /// </summary>
        /// <param name="prefs"Имя префа</param>
        /// <returns>Преф или null если префа нет</returns>
        public PrefsValue FindPrefValue(Prefs prefs)
        {
            return prefsValues.Find(x => x.pref == prefs);
        }



        public void PrefNotFoundError(Prefs prefs)
        {
            Debug.LogError("Yaroslav: Pref \"" + prefs.ToString() + "\" not found in SavesData list");
        }
        public void SetNames()
        {
            foreach (var item in prefsValues)
            {
                item.name = item.pref.ToString();
            }
        }

        public override void Save()
        {
            
        }
    };
}