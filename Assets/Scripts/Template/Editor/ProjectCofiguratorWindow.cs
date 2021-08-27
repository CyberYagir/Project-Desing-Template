using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectCofiguratorWindow : EditorWindow
{
    [MenuItem("Tools/Yaroslav/Configurator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ProjectCofiguratorWindow));
    }

    private void OnFocus()
    {
        EditorWindow.GetWindow(typeof(ProjectCofiguratorWindow)).minSize = new Vector2(500, 300);
    }
    void OnGUI()
    {
        if (GUILayout.Button("Configure Resources"))
        {
            bool creatingGameData = false;
            if (Resources.Load<GameDataObject>("GameData") == null)
            {
                AssetDatabase.CreateAsset(new GameDataObject(), "Assets/Resources/GameData.asset");
                creatingGameData = true;
                Debug.Log("Yaroslav: GameData created!");
            }
            if (Resources.Load<GameDataObject>("SavesData") == null)
            {
                AssetDatabase.CreateAsset(new SavesDataObject(), "Assets/Resources/SavesData.asset");
                if (creatingGameData == true)
                    GameDataObject.GetMain().saves = Resources.Load<SavesDataObject>("SavesData");
                Debug.Log("Yaroslav: SavesData created!");
            }
            if (Resources.Load<GameDataObject>("GameTypes") == null)
            {
                AssetDatabase.CreateAsset(new GameDatasManagerObject(), "Assets/Resources/GameTypes.asset");
                Debug.Log("Yaroslav: GameTypes created!");
            }
            AssetDatabase.SaveAssets();
        }
    }
}
