using System;
using Template.Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class LoaderController : MonoBehaviour
    {
        [SerializeField] private GameDataObject gameData;
        private void Awake()
        {
            gameData.Saves.Load();
            SceneManager.LoadScene(gameData.Levels[gameData.Saves.LevelData.Level]);
        }
    }
}
