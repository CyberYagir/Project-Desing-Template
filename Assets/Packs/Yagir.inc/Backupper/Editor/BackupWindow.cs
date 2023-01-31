using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static YagirDev.AssetsToSaveHolder;
using static YagirDev.BackupperManager;
using static YagirDev.BackupperPrefs;
using static YagirDev.BackupperPrefs.EditorPrefsValues;
using Object = UnityEngine.Object;

namespace YagirDev
{
    public class BackupWindow : EditorWindow
    {
        public static bool IsWindowShowed = false;

        public static AssetsToSaveHolder AssetToSaveHolder;
        
        private string selectedPathToRestore = "";
        private int currentTab = 0;
        private Vector2 scroll;
        private Vector2 scroll2;
        
        private Dictionary<SaveInFolderTypes, AssetsDisplay> assetsDatas = new Dictionary<SaveInFolderTypes, AssetsDisplay>();
        
        
        [MenuItem("Tools/Backup Options")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(BackupWindow)) as BackupWindow;
            EditorWindow.GetWindow(typeof(BackupWindow)).titleContent = new GUIContent("Backupper");
            CreateBackupsFolders();
            IsWindowShowed = true;
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(700, 700);
            window.Init();
        }

        public void Init()
        {
            assetsDatas = GetMaskData();
        }


        public static Dictionary<SaveInFolderTypes, AssetsDisplay> GetMaskData()
        {
            var dir = new Dictionary<SaveInFolderTypes, AssetsDisplay>();
            dir.Add(SaveInFolderTypes.AnimatorController, new AssetsDisplay("t:AnimatorController", EditorGUIUtility.IconContent("d_AnimatorController Icon")));
            dir.Add(SaveInFolderTypes.Materials, new AssetsDisplay("t:Material", EditorGUIUtility.IconContent("d_Material Icon")));
            dir.Add(SaveInFolderTypes.Prefabs, new AssetsDisplay("t:Prefab", EditorGUIUtility.IconContent("d_Prefab Icon")));
            dir.Add(SaveInFolderTypes.Animations, new AssetsDisplay("t:Animation", EditorGUIUtility.IconContent("d_AnimationClip Icon")));
            dir.Add(SaveInFolderTypes.ScriptableObject, new AssetsDisplay("t:ScriptableObject", EditorGUIUtility.IconContent("d_ScriptableObject Icon")));
            return dir;
        }
        
        private void OnGUI()
        {
            IsWindowShowed = true;
            currentTab = GUILayout.Toolbar(currentTab, new string[] {"Options", "Browser", "Restore", "Custom Assets"});
            switch (currentTab)
            {
                case 0:
                    DrawOptions();
                    break;
                case 1:
                    DrawBackups();
                    break;
                case 2:
                    DrawRestore();
                    break;
                case 3:
                    DrawCustom();
                    break;
            }
        }

        private void DrawCustom()
        {
            if (FindCustomHolder())
            {
                if (GUILayout.Button("Add Folder"))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                    path = ChangeFolderPath(path);
                    if (path != "")
                    {
                        AssetToSaveHolder.foldersToSave.Add(new FolderOptions(path));
                    }
                }
                
                scroll = GUILayout.BeginScrollView(scroll);
                for (int i = 0; i < AssetToSaveHolder.foldersToSave.Count; i++)
                {
                    var folder = AssetToSaveHolder.foldersToSave[i];
                    DrawFolderObject(folder);
                    if (!AssetToSaveHolder.foldersToSave.Contains(folder))
                    {
                        return;
                    }
                }
                GUILayout.EndScrollView();
            }
        }


        public string ChangeFolderPath(string path)
        {
            if (path != "" && path != "Assets")
            {
                path = path.Replace(Application.dataPath, "Assets");

                if (AssetDatabase.IsValidFolder(path) || AssetDatabase.LoadAssetAtPath<Object>(path) != null)
                {
                    GUI.FocusControl(null);
                    AssetToSaveHolder.SetDirty();
                    AssetDatabase.SaveAssetIfDirty(AssetToSaveHolder);
                    return path;
                }
            }
            return String.Empty;
        }
        
        private void DrawFolderObject(FolderOptions folder)
        {
            GUILayout.BeginVertical("Box");
            {
                GUILayout.BeginHorizontal();
                {
                    folder.isShow = EditorGUILayout.Foldout(folder.isShow, folder.folderPath, true);
                    if (GUILayout.Button("Change Folder", GUILayout.MaxWidth(100)))
                    {
                        string path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                        path = ChangeFolderPath(path);
                        
                        if (path != "")
                        {
                            folder.folderPath = path;
                        }
                    }
                    if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                    {
                        AssetToSaveHolder.foldersToSave.Remove(folder);
                        return;
                    }
                }
                GUILayout.EndHorizontal();

                if (folder.isShow)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(20);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(5);
                            folder.types = (SaveInFolderTypes) EditorGUILayout.MaskField("Objects Type: ", (int)folder.types, Enum.GetNames(typeof(SaveInFolderTypes)));
                            UpdateEnum(folder);

                            
                            folder.isShowItems = EditorGUILayout.Foldout(folder.isShowItems, "Items", true);
                            if (folder.isShowItems)
                            {
                                if (assetsDatas.Count == 0)
                                {
                                    Init();
                                }

                                if (folder.types.HasFlag(SaveInFolderTypes.CustomFiles) == false)
                                {
                                    DrawItemsByType(folder);
                                }
                                else
                                {
                                    DrawCustomItemsList(folder);
                                }
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            
        }

        public void DrawCustomItemsList(FolderOptions folder)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(15);

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Add"))
                    {
                        string path = EditorUtility.OpenFilePanel("Select File", Application.dataPath, "");

                        path = ChangeFolderPath(path);
                        if (path != "")
                        {
                            if (!folder.customFiles.Contains(path))
                            {
                                folder.customFiles.Add(path);
                            }
                        }

                    }

                    for (var i = 0; i < folder.customFiles.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUIContent icon = null;

                            switch (Path.GetExtension(folder.customFiles[i]))
                            {
                                case ".controller":
                                    icon = assetsDatas[SaveInFolderTypes.AnimatorController].icon;
                                    break;
                                case ".anim":
                                    icon = assetsDatas[SaveInFolderTypes.Animations].icon;
                                    break;
                                case ".mat":
                                    icon = assetsDatas[SaveInFolderTypes.Materials].icon;
                                    break;
                                case ".prefab":
                                    icon = assetsDatas[SaveInFolderTypes.Prefabs].icon;
                                    break;
                                case ".asset":
                                    icon = assetsDatas[SaveInFolderTypes.ScriptableObject].icon;
                                    break;
                                default:
                                    icon = EditorGUIUtility.IconContent("Collab.FileAdded");
                                    break;
                            }

                            GUILayout.Label(icon, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                            folder.customFiles[i] = EditorGUILayout.TextField(folder.customFiles[i]);
                            if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                            {
                                folder.customFiles.RemoveAt(i);
                                GUI.FocusControl(null);
                                return;
                            }
                        }   
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        
        public void DrawItemsByType(FolderOptions folder)
        {
            GUILayout.Space(10);
            foreach (var data in assetsDatas)
            {
                if (folder.types.HasFlag(data.Key))
                {
                    var files = AssetDatabase.FindAssets(data.Value.filter, new[] {folder.folderPath + "/"});
                    DrawFilesList(files, data.Key);
                }
            }
        }

        public void DrawFilesList(string[] files, SaveInFolderTypes key)
        {
            foreach (var file in files)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUILayout.Label(assetsDatas[key].icon, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                    GUILayout.Label(AssetDatabase.GUIDToAssetPath(file).Replace("Assets/", ""));
                }
                GUILayout.EndHorizontal();
            }
        }
        

        private void UpdateEnum(FolderOptions folder)
        {
            if (HaveAllEnumSelected(folder))
            {
                folder.types = SaveInFolderTypes.AnimatorController | SaveInFolderTypes.Animations | SaveInFolderTypes.Materials | SaveInFolderTypes.Prefabs | SaveInFolderTypes.ScriptableObject;
            }
            if (folder.types.HasFlag(SaveInFolderTypes.CustomFiles))
            {
                folder.types = SaveInFolderTypes.CustomFiles;
            }
        }

        private bool HaveAllEnumSelected(FolderOptions folder)
        {
            return folder.types.HasFlag(SaveInFolderTypes.Animations) &&
                   folder.types.HasFlag(SaveInFolderTypes.Materials) &&
                   folder.types.HasFlag(SaveInFolderTypes.Prefabs) &&
                   folder.types.HasFlag(SaveInFolderTypes.ScriptableObject) &&
                   folder.types.HasFlag(SaveInFolderTypes.AnimatorController) &&
                   folder.types.HasFlag(SaveInFolderTypes.CustomFiles);
        }

        public const string HolderLabel = "BackupperHolder";
        private bool FindCustomHolder()
        {
            if (AssetToSaveHolder == null)
            {
                var files = AssetDatabase.FindAssets($"l:{HolderLabel}");
                if (files.Length == 0)
                {
                    if (GUILayout.Button("CreateCustomFile"))
                    {
                        if (!AssetDatabase.IsValidFolder("Assets/Editor"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Editor");
                        }

                        var holder = ScriptableObject.CreateInstance<AssetsToSaveHolder>();
                        AssetDatabase.CreateAsset(holder, "Assets/Editor/AssetsToSaveHolder.asset");
                        AssetDatabase.SetLabels(holder, new[] {HolderLabel});
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        AssetToSaveHolder = holder;
                    }
                }
                else
                {
                    AssetToSaveHolder = AssetDatabase.LoadAssetAtPath<AssetsToSaveHolder>(AssetDatabase.GUIDToAssetPath(files[0]));
                }
            }

            return AssetToSaveHolder != null;
        }

        private bool isScenesShow, isPrefabsShow, isAnimatorsShow, isAnimsShow, isScriptableShow, isMaterialsShow, isOtherShow;

        private List<string> scenesList;
        private List<string> prefabsList;
        private List<string> animatorsList;
        private List<string> animsList;
        private List<string> scriptableList;
        private List<string> materialsList;
        private List<string> other;


        private BackupVisuals restoreVisuals;
        public void SortRestoreFiles()
        {
            if (selectedPathToRestore != "")
            {
                if (Directory.Exists(selectedPathToRestore))
                {
                    restoreVisuals = new BackupVisuals(selectedPathToRestore);
                    
                    scenesList = GetFilesListWithoutEx(restoreVisuals, ".unity");
                    prefabsList = GetFilesListWithoutEx(restoreVisuals, ".prefab");
                    animatorsList = GetFilesListWithoutEx(restoreVisuals, ".controller");
                    animsList = GetFilesListWithoutEx(restoreVisuals, ".anim");
                    scriptableList = GetFilesListWithoutEx(restoreVisuals, ".asset");
                    materialsList = GetFilesListWithoutEx(restoreVisuals, ".mat");
                    other = restoreVisuals.data.savedFiles.paths;
                }
            }
        }
        
        private void DrawRestore()
        {
            if (selectedPathToRestore != "" && restoreVisuals != null)
            {
                if (Directory.Exists(selectedPathToRestore))
                {
                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        GUILayout.Label("Backup " + restoreVisuals.time);

                        DrawRestoreFoldout(ref isScenesShow, "Scenes", scenesList, "d_UnityLogo");
                        DrawRestoreFoldout(ref isPrefabsShow, "Prefabs", prefabsList, "d_Prefab Icon");
                        DrawRestoreFoldout(ref isAnimatorsShow, "Animators", animatorsList, "d_AnimatorController Icon");
                        DrawRestoreFoldout(ref isAnimsShow, "Animations", animsList, "d_AnimationClip Icon");
                        DrawRestoreFoldout(ref isMaterialsShow, "Materials", materialsList, "d_Material Icon");
                        DrawRestoreFoldout(ref isScriptableShow, "Scriptable", scriptableList, "d_ScriptableObject Icon");
                        DrawRestoreFoldout(ref isOtherShow, "Other", other, "Collab.FileAdded");
                        
                    }
                    GUILayout.EndScrollView();
                }
                else
                {
                    selectedPathToRestore = "";
                }
            }
            else
            {
                GUILayout.Label("Select backup...");
            }
        }

        public void DrawRestoreFoldout(ref bool state, string text, List<string> list, string icon)
        {
            if (list.Count != 0)
            {
                state = EditorGUILayout.Foldout(state, text + $" [{list.Count}]", true);
                if (state)
                {
                    DrawFilesRestore(list, EditorGUIUtility.IconContent(icon));
                }
            }
        }

        private void DrawFilesRestore(List<string> list, GUIContent icon)
        {
            foreach (var asset in list)
            {
                GUILayout.BeginHorizontal("Box");
                {
                    GUILayout.Label(icon, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));
                    GUILayout.Label(asset);
                    if (GUILayout.Button("Restore", GUILayout.MaxWidth(100)))
                    {
                        if (Path.GetExtension(asset) == ".asset")
                        {
                            var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(asset);
                            if (obj != null)
                            {
                                AssetDatabase.SaveAssetIfDirty(obj);
                                AssetDatabase.Refresh();
                            }
                        }
                        Restore(asset, selectedPathToRestore);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        
        private List<string> GetFilesListWithoutEx(BackupVisuals visuals, string ex)
        {
            var list = visuals.data.savedFiles.paths.FindAll(x => Path.GetExtension(x) == ex);
            visuals.data.savedFiles.paths.RemoveAll(x => Path.GetExtension(x) == ex);

            return list;
        }

        private void DrawBackups()
        {
            DrawBackupsList();
            DrawScenesToCurrentBackup();
        }
        
        private void DrawBackupsList()
        {
            int drawCount = EditorPrefs.GetInt("BackupsDraw", 20);
            EditorGUILayout.BeginVertical("Box");
            {
                var backups = GetBackupsList();

                scroll = GUILayout.BeginScrollView(scroll);
                {
                    for (int i = 0; i < Mathf.Clamp(backups.Count, 0, drawCount); i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(backups[i].time, GUILayout.MaxWidth(150));
                            GUILayout.Space(10);
                            if (backups[i].time != "Empty")
                            {
                                if (GUILayout.Button(selectedPathToRestore == backups[i].path && restoreVisuals != null ? "Selected" : $"Restore [{backups[i].data.savedFiles.paths.Count}]"))
                                {
                                    selectedPathToRestore = backups[i].path;
                                    currentTab = 2;
                                    SortRestoreFiles();
                                    return;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (backups.Count == 0)
                    {
                        GUILayout.Label("All backups is empty..");
                    }
                }
                GUILayout.EndScrollView();

                drawCount = EditorGUILayout.IntSlider("Last Draw Count: ", drawCount, 10, BackupsCount);
                EditorPrefs.SetInt("BackupsDraw", drawCount);

            }
            EditorGUILayout.EndVertical();
        }

        private List<BackupVisuals> GetBackupsList()
        {
            List<BackupVisuals> backups = new List<BackupVisuals>();
            for (int i = 0; i < BackupsCount; i++)
            {
                backups.Add(new BackupVisuals(GetBackupPathByID(i)));
            }
            backups = backups.OrderBy(x => x.dateTime).ToList();
            backups.Reverse();
            backups.RemoveAll(x => x.time == "Empty");

            return backups;
        }

        private void OnDisable()
        {
            IsWindowShowed = false;
        }

        private void DrawScenesToCurrentBackup()
        {
            EditorGUILayout.BeginVertical("Box");
            var editedScenes = OpenedData;
            GUILayout.Label("Scenes to next backup: ");
            GUILayout.Space(10);
            if (editedScenes != null && editedScenes.paths.Count != 0)
            {
                for (int i = 0; i < editedScenes.paths.Count; i++)
                {
                    GUILayout.Label($"      [{i + 1}] - " + editedScenes.paths[i]);
                }
            }
            else
            {
                GUILayout.Label("Save scene to backup");
            }

            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }

        private void DrawOptions()
        {
            {
                EditorGUI.BeginChangeCheck();
                {
                    EditorPrefs.SetInt(Pref(MaxBackupsNumber), EditorGUILayout.IntSlider("Backups Count: ", EditorPrefs.GetInt(Pref(MaxBackupsNumber), (int)StartValues[MaxBackupsNumber]), 1, 100));
                }
                if (EditorGUI.EndChangeCheck())
                {
                    CreateBackupsFolders();
                }
            }

            EditorPrefs.SetInt(Pref(BackupTime), EditorGUILayout.IntSlider("Backup Time (seconds): ", EditorPrefs.GetInt(Pref(BackupTime), (int)StartValues[BackupTime]), 10, 1200));
            
            DrawCounter();
            DrawOpenFolder();
            DrawReset();
            DrawClearBackups();
        }

        private void DrawClearBackups()
        {
            if (GUILayout.Button("Clear Backups"))
            {
                var path = Application.persistentDataPath + "/Backups";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    CreateBackupsFolders();
                }
            }
        }
        
        private void DrawReset()
        {
            if (GUILayout.Button("Reset Settings"))
            {
                EditorPrefs.DeleteKey(Pref(BackupNumber));
                EditorPrefs.DeleteKey(Pref(MaxBackupsNumber));
                EditorPrefs.DeleteKey(Pref(BackupTime));
                EditorPrefs.SetString(Pref(StartTime), DateTime.Now.ToString("G"));
            }
        }
        
        private void DrawCounter()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical("Box");
            {
                var seconds = (BackupDelay - (DateTime.Now - ConvertedStartTime).TotalSeconds);
                GUILayout.Label("Time to save: " + seconds.ToString("F2"));

                if (BackupDelay - seconds < 0)
                {
                    GUILayout.Label("[wait for window close]");
                }
            }
            GUILayout.EndVertical();
        }

        private void DrawOpenFolder()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Backups Folder..."))
            {
                Application.OpenURL(Application.persistentDataPath);
            }
        }
    }
}
