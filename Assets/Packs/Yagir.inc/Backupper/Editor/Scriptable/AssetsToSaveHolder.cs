using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YagirDev
{
    public class AssetsToSaveHolder : ScriptableObject
    {
        [System.Flags]
        public enum SaveInFolderTypes
        {
            AnimatorController = 0x01,
            ScriptableObject = 0x02,
            Animations = 0x04,
            Materials = 0x08,
            Prefabs = 0x010,
            CustomFiles = 0x20
        }

        [System.Serializable]
        public class FolderOptions
        {
            public SaveInFolderTypes types;
            public string folderPath;
            public List<string> customFiles = new List<string>();

            public bool isShow = false;
            public bool isShowItems = false;
            
            public FolderOptions(){}

            public FolderOptions(string folderPath)
            {
                this.folderPath = folderPath;
            }

        }

        
        public List<FolderOptions> foldersToSave;
    }
}
