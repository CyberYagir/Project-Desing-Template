using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "SavesData", menuName = "Yaroslav/SavesData Json", order = 2)]
    public class SavesDataObjectJson : AbstractSavesDataObject
    {
        public static string Path;
        [System.Serializable]
        public class PrefData
        {
            public PrefType type;
            public int intVal;
            public float floatVal;
            public string strVal;
            public bool boolVal;
        }
        [System.Serializable]
        public class SaveData
        {
            public List<Prefs> prefsValue = new List<Prefs>();
            public List<PrefData> prefsData = new List<PrefData>();

            public Dictionary<Prefs, PrefData> GetDictionary()
            {
                var dic = new Dictionary<Prefs, PrefData>();
                for (int i = 0; i < Enum.GetNames(typeof(Prefs)).Length; i++)
                {
                    dic.Add(prefsValue[i], prefsData[i]);
                }
                return dic;
            }

            public void FromDictionary(Dictionary<Prefs, PrefData> prefDatas)
            {
                prefsValue = new List<Prefs>();
                prefsData = new List<PrefData>();
                foreach (var data in prefDatas)
                {
                    prefsValue.Add(data.Key);
                    prefsData.Add(data.Value);
                }
            }
        }

        [HideInInspector]
        public SaveData saveData;

        public Dictionary<Prefs, PrefData> prefsData;

        
        private void Awake()
        {
            LoadCheck();
        }

        public void SetPath()
        {
            Path = Application.persistentDataPath + "/Save.json";
        }

        public void Init()
        {
            SetPath();
            if (File.Exists(Path))
            {
                try
                {
                    saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(Path));
                }catch (Exception e){ }
                
                
                if (saveData != null && saveData.prefsValue.Count != 0)
                {
                    prefsData = saveData.GetDictionary();
                    AddPrefs();
                }
                else
                {
                    Clear();
                }
            }
            else
            {
                Save();
            }

            if (prefsData == null)
            {
                Clear();   
            }
            
        }


        public void LoadCheck()
        {
            if (prefsData == null)
            {
                Init();
            }
        }

        public void AddPrefs()
        {
            LoadCheck();
            var prefs = Enum.GetValues(typeof(Prefs)).Length;
            for (int i = 0; i < prefs; i++)
            {
                var pref = (Prefs)i;
                if (!prefsData.ContainsKey(pref))
                {
                    prefsData.Add(pref, new PrefData() { type = PrefType.Int});
                    switch (prefsData[pref].type)
                    {
                        case PrefType.String:
                            prefsData[pref].strVal = String.Empty;
                            break;
                        case PrefType.Int:
                            prefsData[pref].intVal = 0;
                            break;
                        case PrefType.Float:
                            prefsData[pref].floatVal = 0f;
                            break;
                        case PrefType.Bool:
                            prefsData[pref].boolVal = false;
                            break;
                    }
                }
            }
        }

        public void Clear()
        {
            if (prefsData == null)
            {
                if (saveData != null && saveData.prefsValue.Count != 0)
                {
                    prefsData = saveData.GetDictionary();
                }
                else
                {
                    prefsData = new Dictionary<Prefs, PrefData>();
                }
            }

            AddPrefs();
            foreach (var data in prefsData)
            {
                var pref = data.Key;
                switch (prefsData[pref].type)
                {
                    case PrefType.String:
                        prefsData[pref].strVal = String.Empty;
                        break;
                    case PrefType.Int:
                        prefsData[pref].intVal = 0;
                        break;
                    case PrefType.Float:
                        prefsData[pref].floatVal = 0f;
                        break;
                    case PrefType.Bool:
                        prefsData[pref].boolVal = false;
                        break;
                }
            }
            Save();
        }

        public override object GetPref(Prefs pref)
        {
            LoadCheck();
            if (prefsData.ContainsKey(pref))
            {
                switch (prefsData[pref].type)
                {
                    case PrefType.String:
                        return prefsData[pref].strVal;
                    case PrefType.Int:
                        return prefsData[pref].intVal;
                    case PrefType.Float:
                        return prefsData[pref].floatVal;
                    case PrefType.Bool:
                        return prefsData[pref].boolVal;
                }
            }
            return 0;
        }

        public override void SetPref(Prefs pref, object value)
        {
            LoadCheck();
            if (prefsData.ContainsKey(pref))
            {
                switch (prefsData[pref].type)
                {
                    case PrefType.String:
                        prefsData[pref].strVal = (string) value;
                        break;
                    case PrefType.Int:
                        prefsData[pref].intVal = (int) value;
                        break;
                    case PrefType.Float:
                        prefsData[pref].floatVal = (float) value;
                        break;
                    case PrefType.Bool:
                        prefsData[pref].boolVal = (bool) value;
                        break;
                }
            }
        }

        public override void AddToPref(Prefs pref, object value)
        {
            LoadCheck();
            if (prefsData.ContainsKey(pref))
            {
                switch (prefsData[pref].type)
                {
                    case PrefType.String:
                        prefsData[pref].strVal += value.ToString();
                        break;
                    case PrefType.Int:
                        if (value is float)
                        {
                            prefsData[pref].intVal += (int)(float)value;
                        }
                        else if (value is int)
                        {
                            prefsData[pref].intVal += (int) value;
                        }
                        break;
                    case PrefType.Float:
                        if (value is float)
                        {
                            prefsData[pref].floatVal += (float)value;
                        }
                        else if (value is int)
                        {
                            prefsData[pref].floatVal += (int) value;
                        }
                        break;
                    case PrefType.Bool:
                        prefsData[pref].boolVal = (bool) value;
                        break;
                }
            }

        }


        public override void SubToPref(Prefs pref, object value)
        {
            LoadCheck();
            if (prefsData.ContainsKey(pref))
            {
                switch (prefsData[pref].type)
                {
                    case PrefType.String:
                        prefsData[pref].strVal = prefsData[pref].strVal.Replace(value.ToString(), "");
                        break;
                    case PrefType.Int:
                        prefsData[pref].intVal -= (int) value;
                        break;
                    case PrefType.Float:
                        prefsData[pref].floatVal -= (float) value;
                        break;
                    case PrefType.Bool:
                        prefsData[pref].boolVal = (bool) value;
                        break;
                }
            }

        }
        
        

        public override void Save()
        {
            SetPath();
            if (prefsData != null)
            {
                saveData.FromDictionary(prefsData);
            }

            var json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            File.WriteAllText(Path, json);
        }
    }
}