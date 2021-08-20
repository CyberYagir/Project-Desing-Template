using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefsValues {
    public SavesDataObject.Prefs pref;
    public SavesDataObject.SavePref savePref;
}



[CreateAssetMenu(fileName = "Saves Data", menuName = "Yaroslav/Saves Data", order = 2)]
public class SavesDataObject : ScriptableObject
{
    public enum Prefs {Level, Points}
    public enum SavePref {String, Int, Float }
    public List<PrefsValues> prefsValues;
        

    public static void SetLevel(int id)
    {
        if (id >= GameDataObject.GetData().levelManagers.Count)
        {
            SetValue(Prefs.Level, 0);
        }
        else
        {
            SetValue(Prefs.Level, id);
        }
    }

    public static object GetFromPrefs(Prefs prefs)
    {
        var prefType = GetData().prefsValues.Find(x => x.pref == prefs).savePref;
        if (PlayerPrefs.HasKey(prefs.ToString()))
        {
            return prefType == SavePref.Int ?
                                        (object)PlayerPrefs.GetInt(prefs.ToString()) :
                                                (prefType == SavePref.Float ? (object)PlayerPrefs.GetFloat(prefs.ToString()) :
                                                        (object)PlayerPrefs.GetString(prefs.ToString()));
        }
        return prefType == SavePref.Int ?
                                        (object)0 :
                                                (prefType == SavePref.Float ? 0.0f :
                                                        (object)"NULL");
    }

    public static void SetValue(Prefs prefs, object value)
    {
        var prefType = GetData().prefsValues.Find(x => x.pref == prefs).savePref;
        switch (prefType)
        {
            case SavePref.String:
                PlayerPrefs.SetString(prefs.ToString(), value.ToString());
                break;
            case SavePref.Int:
                PlayerPrefs.SetInt(prefs.ToString(), (int)value);
                break;
            case SavePref.Float:
                PlayerPrefs.SetFloat(prefs.ToString(), (float)value);
                break;
            default:
                Debug.LogError("Save Data: Save Case Error; Type not found");
                break;
        }
    }

    public static SavesDataObject GetData()
    {
        return Resources.Load<SavesDataObject>("SavesData");
    }
}
