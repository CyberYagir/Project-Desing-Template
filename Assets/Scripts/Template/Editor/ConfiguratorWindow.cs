using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class ConfiguratorWindow : EditorWindow
{
    [MenuItem("Yaroslav/Configurator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ConfiguratorWindow));
        EditorWindow.GetWindow(typeof(ConfiguratorWindow)).titleContent = new GUIContent("Project Configurator");
    }
    public void DrawLine(int h = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, h);
        rect.height = h;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
    public void DrawSeparator()
    {
        GUILayout.Space(10);
        DrawLine();
        GUILayout.Space(10);
    }
    private void OnFocus()
    {
        EditorWindow.GetWindow(typeof(ConfiguratorWindow)).minSize = new Vector2(200, 300);
    }
    [MenuItem("Yaroslav/Move Scripts", false, priority = 11)]
    public static void MoveScriptsFromAssets()
    {
        var scripts = AssetDatabase.FindAssets("t:Script", new string[] { "Assets" });
        CreateFolder("Scripts");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        foreach (var item in scripts)
        {
            var path = AssetDatabase.GUIDToAssetPath(item);
            if (Path.GetDirectoryName(path) == "Assets")
            {
                if (!Path.GetFileName(path).Contains("Manager"))
                {
                    AssetDatabase.MoveAsset(path, "Assets/Scripts/" + Path.GetFileName(path));
                }
                else
                {
                    if (AssetDatabase.IsValidFolder("Assets/Scripts/Template/Managers"))
                    {
                        AssetDatabase.MoveAsset(path, "Assets/Scripts/Template/Managers/" + Path.GetFileName(path));
                    }
                    else
                    {
                        CreateFolder("Scripts/Managers");
                        AssetDatabase.MoveAsset(path, "Assets/Scripts/Managers/" + Path.GetFileName(path));
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Configure Resources"))
        {
            ConfigureResourcesBtn();
        }
        if (GUILayout.Button("Configure Graphics"))
        {
            ConfigurateGraphicsBtn();
        }
        if (GUILayout.Button("Configure Folders"))
        {
            FoldersConfiguration();
        }
        if (GUILayout.Button("Configure All"))
        {
            ConfigurateAllBtn();
        }
        DrawSeparator();
        if (GUILayout.Button("Separate Scripts to folders"))
        {
            MoveScriptsFromAssets();
        }
    }

    public void ConfigurateAllBtn()
    {
        ConfigurateGraphicsBtn();
        ConfigureResourcesBtn();
        FoldersConfiguration();
        MoveScriptsFromAssets();
    }

    public void ConfigurateFoldersBtn()
    {
        FoldersConfiguration();
    }
    public void ConfigurateGraphicsBtn()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
        QualitySettings.pixelLightCount = 0;
        QualitySettings.masterTextureLimit = 0;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.antiAliasing = 0;
        QualitySettings.softParticles = false;
        QualitySettings.realtimeReflectionProbes = false;
        QualitySettings.billboardsFaceCameraPosition = false;
        QualitySettings.resolutionScalingFixedDPIFactor = 1;
        QualitySettings.streamingMipmapsActive = false;
        //Shadows
        QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
        QualitySettings.shadows = ShadowQuality.HardOnly;
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        QualitySettings.shadowProjection = ShadowProjection.CloseFit;
        QualitySettings.shadowDistance = 80;
        QualitySettings.shadowNearPlaneOffset = 3;
        QualitySettings.shadowCascades = 2;
        //Others
        QualitySettings.skinWeights = SkinWeights.OneBone;
        QualitySettings.vSyncCount = 0;

    }
    public void FoldersConfiguration()
    {
        CreateFolder("Animations");

        CreateFolder("Images");
        CreateFolder("Images/Icons");
        CreateFolder("Images/Sprites");
        CreateFolder("Images/Textures");


        CreateFolder("Resources");

        CreateFolder("Models");

        CreateFolder("Materials");

        CreateFolder("Packs");

        CreateFolder("Prefabs");
        CreateFolder("Prefabs/Levels");

        CreateFolder("Resources");

        CreateFolder("Scenes");

        CreateFolder("Scripts");

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    public static void CreateFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder("Assets/" + path))
        {
            var assetsPath = "Assets/";
            if(Path.GetDirectoryName(path) == "")
            {
                assetsPath = "Assets";
            }

            AssetDatabase.CreateFolder(assetsPath + Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    [MenuItem("Yaroslav/Select GameData", false, priority = 10)]
    public static void SelectGameData()
    {
        EditorGUIUtility.PingObject(GameDataObject.GetData());
    }
    public void ConfigureResourcesBtn()
    {
        CreateFolder("Resources");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        string[] unusedFolder = { "Assets/Resources" };
        foreach (var asset in AssetDatabase.FindAssets("", unusedFolder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.MoveAssetToTrash(path);
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        SavesDataObject saves = null;
        if (Resources.Load<AbstractSavesDataObject>("SavesData") == null)
        {
            saves = new SavesDataObject();
            AssetDatabase.CreateAsset(saves, "Assets/Resources/SavesData.asset");
            saves = Resources.Load<SavesDataObject>("SavesData");

            var prefs = Enum.GetValues(typeof(Prefs)).Cast<Prefs>().ToList();
            for (int i = 0; i < prefs.Count; i++)
            {
                saves.prefsValues.Add(new PrefsValue() { pref = (Prefs)i, savePref = PrefType.Int });
            }
            EditorUtility.SetDirty(saves);
            AssetDatabase.SaveAssets();
            Debug.Log($"Yaroslav: SavesData created! [Prefs added {saves.prefsValues.Count}]");
        }

        if (Resources.Load<GameDataObject>("GameData") == null)
        {
            var gameData = new GameDataObject();
            AssetDatabase.CreateAsset(gameData, "Assets/Resources/GameData.asset");
            gameData.main = GetAllDataFromAssets();
            gameData.main.saves = saves;
            gameData.main.levelList = gameData.main.levelList.OrderBy(x => x.name).ToList();

            EditorUtility.SetDirty(gameData);
            AssetDatabase.SaveAssets();
            Debug.Log("Yaroslav: GameData created!");
        }

        if (Resources.Load<GameDataObject>("GameTypes") == null)
        {
            AssetDatabase.CreateAsset(new GameDatasManagerObject() /*{ saves = saves}*/, "Assets/Resources/GameTypes.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Yaroslav: GameTypes created!");
        }
        AssetDatabase.SaveAssets();
    }


    public static GameDataObject.GDOMain GetAllDataFromAssets()
    {
        var final = new GameDataObject.GDOMain();
        var prefabs = AssetDatabase.FindAssets("t:prefab");
        foreach (var prefab in prefabs)
        {
            var player = AssetDatabase.LoadAssetAtPath<Player>(AssetDatabase.GUIDToAssetPath(prefab));
            var canvas = AssetDatabase.LoadAssetAtPath<Canvas>(AssetDatabase.GUIDToAssetPath(prefab));
            var level = AssetDatabase.LoadAssetAtPath<LevelManager>(AssetDatabase.GUIDToAssetPath(prefab));
            if (player != null)
            {
                if (final.playerPrefab == null)
                {
                    final.playerPrefab = player.gameObject;
                }
            }
            else if (canvas != null)
            {
                if (final.canvas == null)
                {
                    final.canvas = canvas.gameObject;
                }
            }
            else if (level != null)
            {
                final.levelList.Add(level);
            }
        }

        return final;
    }
}
