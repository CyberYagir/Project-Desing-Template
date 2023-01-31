using System;
using System.IO;
using UnityEngine;

namespace YagirDev
{
    public class BackupVisuals
    {
        public string path;
        public string time;
        public DateTime dateTime = new DateTime(2000, 1, 1, 1, 1, 1);
        public BackupperManager.BackupData data;

        public BackupVisuals(string path)
        {
            this.path = path;
            var dataPath = path + "/data.json";
            if (File.Exists(dataPath))
            {
                data = JsonUtility.FromJson<BackupperManager.BackupData>(File.ReadAllText(dataPath));
                if (data == null || data.backupTime == null)
                {
                    time = "Empty";
                    return;
                }

                time = data.backupTime;
                dateTime = DateTime.Parse(time);
            }
            else
            {
                time = "Empty";
            }
        }
    }
}