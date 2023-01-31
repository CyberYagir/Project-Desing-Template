using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static YagirDev.BackupperPrefs;
using static YagirDev.BackupperPrefs.EditorPrefsValues;
using EditorPrefs = UnityEditor.EditorPrefs;

namespace YagirDev
{
    [InitializeOnLoad]
    public static partial class BackupperManager
    {
        public static int BackupIndex => EditorPrefs.GetInt(Pref(BackupNumber), (int) StartValues[BackupNumber]);
        public static int BackupsCount => EditorPrefs.GetInt(Pref(MaxBackupsNumber), (int) StartValues[MaxBackupsNumber]);
        public static int BackupDelay => EditorPrefs.GetInt(Pref(BackupTime), (int) StartValues[BackupTime]);

        public static OpenedFiles OpenedData = null;
        public static readonly string OpenedDataJsonPath;

        private static string OpenedScenePath = "";

        static BackupperManager()
        {
            if (SessionState.GetBool("Started", false) == false)
            {
                EditorPrefs.SetString("startTime", DateTime.Now.ToString("G"));
                SessionState.SetBool("Started", true);
            }

            OpenedDataJsonPath = Application.persistentDataPath + $"/Backups/OpenedScenes.json";

            CreateBackupsFolders();
            CheckOpenedScenes();
            EditorApplication.update += ExecuteCoroutine;
        }

        
        #region FileSystem

        public static string GetBackupPathByID(int i)
        {
            return Application.persistentDataPath + $"/Backups/Backup_{i}";
        }

        public static void CreateBackupsFolders()
        {
            for (int i = 0; i < BackupsCount; i++)
            {
                string path = GetBackupPathByID(i);
                CreateFolder(path);
            }
        }

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + "/data.json"))
            {
                File.WriteAllText(path + "/data.json", "{}");
            }
        }

        public static void Restore(string path, string backupPath)
        {
            var root = Directory.GetParent(Application.dataPath);
            var source = backupPath + "/" + path;
            var destination = root + "/" + path;
            if (File.Exists(destination))
            {
                var backupSceneData = File.ReadAllText(source);
                var targetSceneData = File.ReadAllText(destination);
                if (backupSceneData != targetSceneData)
                {
                    File.WriteAllText(destination, backupSceneData);

                    AssetDatabase.Refresh();
                    if (EditorSceneManager.GetActiveScene().path == path)
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Scenes have no difference", "OK");
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(source, destination);
                AssetDatabase.Refresh();
            }
        }

        public static void CheckOpenedScenes()
        {
            if (OpenedData == null)
            {
                if (File.Exists(OpenedDataJsonPath))
                {
                    var json = File.ReadAllText(OpenedDataJsonPath);
                    if (json != "")
                    {
                        OpenedData = JsonUtility.FromJson<OpenedFiles>(json);
                        return;
                    }
                }
                else
                {
                    AddOpenedScene();
                }
            }
        }

        public static void AddOpenedScene()
        {
            var path = Application.persistentDataPath + $"/Backups/OpenedScenes.json";
            var currentScene = EditorSceneManager.GetActiveScene().path;
            if (OpenedData == null)
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, JsonUtility.ToJson(new OpenedFiles(), true));
                }

                var json = File.ReadAllText(path);
                if (json != "")
                {
                    OpenedData = JsonUtility.FromJson<OpenedFiles>(json);
                }
                else
                {
                    OpenedData = new OpenedFiles();
                }
            }


            if (string.IsNullOrEmpty(EditorSceneManager.GetActiveScene().path) == false)
            {
                AddOpenedFile(currentScene, path);
            }
        }

        public static void AddOpenedFile(string fileName, string path)
        {
            if (!OpenedData.paths.Contains(fileName))
            {
                OpenedData.paths.Add(fileName);
                var json = JsonUtility.ToJson(OpenedData, true);
                File.WriteAllText(path, json);
            }
        }

        public static void SaveAllInCurretBackup()
        {
            if (OpenedData == null)
            {
                CheckOpenedScenes();
            }
            string path = Application.persistentDataPath + $"/Backups/Backup_{BackupIndex}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            CreateFolder(path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            
            AddFilesFromHolder();
            

            List<string> deleted = new List<string>();
            foreach (var scene in OpenedData.paths)
            {
                var dir = path + "/" + Path.GetDirectoryName(scene);
                var filePath = Directory.GetParent(Application.dataPath) + "/" + scene;
                if (File.Exists(filePath))
                {
                    Directory.CreateDirectory(dir);
                    File.Copy(filePath, dir + "/" + Path.GetFileName(scene));
                }
                else
                {
                    deleted.Add(scene);
                }
            }

            for (int i = 0; i < deleted.Count; i++)
            {
                OpenedData.paths.Remove(deleted[i]);
            }
            OpenedScenePath = "";
            
            File.WriteAllText(path + "/data.json", JsonUtility.ToJson(new BackupData(DateTime.Now, OpenedData)));
            ClearOpenedFiles();
            


            Debug.Log("[Backup Saved]");
        }



        public static void AddFilesFromHolder()
        {
            var files = AssetDatabase.FindAssets($"l:{BackupWindow.HolderLabel}");
            if (files.Length != 0)
            {
                var holder = AssetDatabase.LoadAssetAtPath<AssetsToSaveHolder>(AssetDatabase.GUIDToAssetPath(files[0]));
                if (holder != null)
                {
                    var patterns = BackupWindow.GetMaskData();
                    foreach (var folder in holder.foldersToSave)
                    {
                        if (AssetDatabase.IsValidFolder(folder.folderPath))
                        {
                            if (folder.types.HasFlag(AssetsToSaveHolder.SaveInFolderTypes.CustomFiles))
                            {
                                for (int i = 0; i < folder.customFiles.Count; i++)
                                {
                                    if (!OpenedData.paths.Contains(folder.customFiles[i]))
                                    {
                                        OpenedData.paths.Add(folder.customFiles[i]);
                                    }
                                }
                                
                            }
                            else
                            {
                                var searchPattern = "";
                                foreach (var data in patterns)
                                {
                                    if (folder.types.HasFlag(data.Key))
                                    {
                                        searchPattern += data.Value.filter + " ";
                                    }
                                }

                                if (searchPattern != "")
                                {
                                    var assets = AssetDatabase.FindAssets(searchPattern, new[] {folder.folderPath + "/"});

                                    for (int i = 0; i < assets.Length; i++)
                                    {
                                        var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                                        if (!OpenedData.paths.Contains(path))
                                        {
                                            OpenedData.paths.Add(path);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        

        public static void ClearOpenedFiles()
        {
            if (OpenedData != null)
            {
                OpenedData = new OpenedFiles();
                var json = JsonUtility.ToJson(OpenedData, true);
                File.WriteAllText(OpenedDataJsonPath, json);
            }
        }

        #endregion

        #region Coroutine

            private static void ExecuteCoroutine()
            {
                if (Application.isPlaying == false && !BackupWindow.IsWindowShowed)
                {
                    UpdateOpenedScene();
                    UpdateTime();
                }
            }

            private static void UpdateTime()
            {
                DateTime currentTime = DateTime.Now;
                if ((currentTime - ConvertedStartTime).TotalSeconds > BackupDelay)
                {
                    EditorPrefs.SetString(Pref(StartTime), DateTime.Now.ToString("G"));
                    if (OpenedData != null && OpenedData.paths.Count != 0)
                    {
                        EditorPrefs.SetInt(Pref(BackupNumber), BackupIndex + 1);
                        if (BackupIndex >= BackupsCount)
                        {
                            EditorPrefs.SetInt(Pref(BackupNumber), 0);
                        }

                        SaveAllInCurretBackup();
                    }
                }
            }

            private static void UpdateOpenedScene()
            {
                var path = EditorSceneManager.GetActiveScene().path;
                if (OpenedScenePath != path)
                {
                    OpenedScenePath = path;
                    AddOpenedScene();
                }
            }

        #endregion
    }
}
