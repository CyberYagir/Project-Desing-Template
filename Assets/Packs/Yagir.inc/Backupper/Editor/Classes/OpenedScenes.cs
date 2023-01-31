using System.Collections.Generic;

namespace YagirDev
{
    public static partial class BackupperManager
    {
        [System.Serializable]
        public class OpenedFiles
        {
            public List<string> paths = new List<string>();
        }
    }
}