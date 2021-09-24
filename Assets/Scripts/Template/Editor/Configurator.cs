using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class Configurator : EditorWindow
{
    [MenuItem("Yaroslav/Configurator")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(Configurator));
        EditorWindow.GetWindow(typeof(Configurator)).titleContent = new GUIContent("Project Configurator");
    }

    private void OnFocus()
    {
        EditorWindow.GetWindow(typeof(Configurator)).minSize = new Vector2(200, 300);
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
        if (GUILayout.Button("Configure All"))
        {
            ConfigurateAllBtn();
        }
    }

    public void ConfigurateAllBtn()
    {
        ConfigurateGraphicsBtn();
        ConfigureResourcesBtn();
    }

    public void ConfigurateGraphicsBtn()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
        QualitySettings.pixelLightCount = 0;
        QualitySettings.masterTextureLimit = 0;
        QualitySettings.anisotropicFiltering =  AnisotropicFiltering.ForceEnable;
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

    public void ConfigureResourcesBtn()
    {
 	AssetDatabase.CreateFolder("Assets", "Resources");
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
        if (Resources.Load<GameDataObject>("SavesData") == null)
        {
            saves = new SavesDataObject();
            AssetDatabase.CreateAsset(saves, "Assets/Resources/SavesData.asset");
            saves = Resources.Load<SavesDataObject>("SavesData");

            var prefs = Enum.GetValues(typeof(Prefs)).Cast<Prefs>().ToList();
            for (int i = 0; i < prefs.Count; i++)
            {
                saves.prefsValues.Add(new PrefsValue() { pref = (Prefs)i, savePref = PrefType.Int });
            }
            Debug.Log($"Yaroslav: SavesData created! [Prefs added {saves.prefsValues.Count}]");
        }

        if (Resources.Load<GameDataObject>("GameData") == null)
        {
            var gameData = new GameDataObject();
            AssetDatabase.CreateAsset(gameData, "Assets/Resources/GameData.asset");
            gameData.main = GetAllDataFromAssets();
            gameData.main.saves = saves;
            gameData.main.levelList = gameData.main.levelList.OrderBy(x => x.name).ToList();

            Debug.Log("Yaroslav: GameData created!");
        }
        
        if (Resources.Load<GameDataObject>("GameTypes") == null)
        {
            AssetDatabase.CreateAsset(new GameDatasManagerObject() /*{ saves = saves}*/, "Assets/Resources/GameTypes.asset");
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
