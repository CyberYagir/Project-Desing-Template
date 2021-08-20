using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
public class FieldGenerator : MonoBehaviour
{
    [SerializeField] Texture2D levelMap;
    public void Generate()
    {
        var gd = GameDataObject.GetData();
        if (gd != null)
        {
            if (levelMap != null)
            {
                var tempList = GetComponent<LevelManager>().tilesHolder.Cast<Transform>().ToList();
                foreach (var child in tempList)
                {
                    DestroyImmediate(child.gameObject);
                }
                for (int x = 0; x < levelMap.width; x++)
                {
                    for (int y = 0; y < levelMap.height; y++)
                    {
                        foreach (var item in gd.generatorItems)
                        {
                            if (ColorEqual(levelMap.GetPixel(x, y), item.color))
                            {
                                for (int i = 0; i < item.prefab.Length; i++)
                                {
                                    var inst = (GameObject)PrefabUtility.InstantiatePrefab(item.prefab[i].gameObject, GetComponent<LevelManager>().tilesHolder);
                                    inst.transform.localPosition = new Vector3(x - (levelMap.width / 2), 0, y - (levelMap.height / 2));
                                    inst.name = $"{item.prefab[i].name} Tile: {x}:{y}";
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Error: Game Data Null");
        }

        bool ColorEqual(Color color1, Color color2)
        {
            float threshold = 0.1f; //Exact value should be found by trying different. Possibly different values for      different colors.
            return (Mathf.Abs(color1.r - color2.r) < threshold
            && Mathf.Abs(color1.g - color2.g) < threshold
            && Mathf.Abs(color1.b - color2.b) < threshold);
        }


        if (GetComponentInChildren<PlayerSpawn>())
        {
            GetComponent<LevelManager>().playerSpawn = GetComponentInChildren<PlayerSpawn>().transform;
        }
        else
        {
            Debug.Log("Error: Player Spawn Null");
        }


        EditorUtility.SetDirty(gameObject);
    }
    bool ColorEqual(Color color1, Color color2)
    {
        float threshold = 0.1f; //Exact value should be found by trying different. Possibly different values for      different colors.
        return (Mathf.Abs(color1.r - color2.r) < threshold
        && Mathf.Abs(color1.g - color2.g) < threshold
        && Mathf.Abs(color1.b - color2.b) < threshold);
    }

}
