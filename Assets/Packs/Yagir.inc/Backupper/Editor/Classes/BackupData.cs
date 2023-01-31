using System;
using System.Collections.Generic;

namespace YagirDev
{
    public static partial class BackupperManager
    {
        [System.Serializable]
        public class BackupData
        {
            public string backupTime;
            public OpenedFiles savedFiles;

            public BackupData(DateTime backupTime, OpenedFiles savedFiles)
            {
                this.backupTime = backupTime.ToString("G");
                this.savedFiles = savedFiles;
            }
        }
    }
}