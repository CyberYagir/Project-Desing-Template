using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavesData", menuName = "Yaroslav/SavesData", order = 2)]
public class SavesDataObject : AbstractSavesDataObject { };

#region Abstract_Enums 
public enum Prefs { Level, Points} //�������� ������� ��� ������ ������
public enum PrefType { String, Int, Float, Bool }
[System.Serializable]
public class PrefsValues
{
    [ReadOnly]
    public string name;
    public Prefs pref;
    public PrefType savePref;
}
public abstract class AbstractSavesDataObject : ScriptableObject
{
    public List<PrefsValues> prefsValues = new List<PrefsValues>();

    public virtual void SetLevel(int id)
    {
        if (id >= GameDataObject.GetMain().levelList.Count)
        {
            SetPref(Prefs.Level, 0);
        }
        else
        {
            SetPref(Prefs.Level, id);
        }
    }

    public virtual object GetPref(Prefs prefs)
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

    public virtual void SetPref(Prefs prefs, object value)
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

    public virtual void AddToPref(Prefs prefs, object value)
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

    public virtual void SubToPref(Prefs prefs, object value)
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

    public void SetNames()
    {
        foreach (var item in prefsValues)
        {
            item.name = item.pref.ToString();
        }
    }
}
#endregion