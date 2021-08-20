using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PrefsValues {
    [ReadOnly]
    public string name;
    public Prefs pref;
    public PrefType savePref;
}

[CreateAssetMenu(fileName = "Saves Data", menuName = "Yaroslav/Saves Data", order = 2)]
public class SavesDataObject : AbstractSavesDataObject { };

public enum Prefs { Level, Points} //Добавить элемент для нового префса
public enum PrefType { String, Int, Float, Bool}

public abstract class AbstractSavesDataObject : ScriptableObject
{
    public List<PrefsValues> prefsValues;


    public virtual void SetLevel(int id)
    {
        if (id >= GameDataObject.GetMain().levelList.Count)
        {
            SetValue(Prefs.Level, 0);
        }
        else
        {
            SetValue(Prefs.Level, id);
        }
    }

    public virtual object GetFromPrefs(Prefs prefs)
    {
        var prefType = prefsValues.Find(x => x.pref == prefs).savePref;
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

        return 0;
    }

    public virtual void SetValue(Prefs prefs, object value)
    {
        var prefType = prefsValues.Find(x => x.pref == prefs).savePref;
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


    public void SetNames()
    {
        foreach (var item in prefsValues)
        {
            item.name = item.pref.ToString();
        }
    }
}
