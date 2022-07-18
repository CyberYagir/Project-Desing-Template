using System;
using System.IO;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "SavesDataJson", menuName = "Yaroslav/SavesDataJson", order = 5)]
    public class SaveDataObjectJson : AbstractSavesDataObject
    {
        private static SaveDataObjectJson defaultValues;
        private string path;

        private void SetPath()
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Application.persistentDataPath + "/Save.json";
            }
        }

        public override void Save()
        {
            SetPath();
            var json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }
        
        public override void Load()
        {
            ResetObject();
            SetPath();
            if (File.Exists(path))
            {
                var json = NonAllocString.instance + File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, this);
            }
        }

        public void ResetObject()
        {
            if (defaultValues == null)
            {
                defaultValues = CreateInstance<SaveDataObjectJson>();
            }
            var json = NonAllocString.instance + JsonUtility.ToJson(defaultValues);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
