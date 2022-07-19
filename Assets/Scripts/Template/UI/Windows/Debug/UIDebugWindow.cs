using System.Collections.Generic;
using Template.Managers;
using Template.Tweaks;
using TMPro;
using UnityEngine;

namespace Template.UI.Windows
{
    public class UIDebugWindow : MonoCustom
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TMP_InputField moneyInput;

        private UIController controller;
        
        public UEvent HideAll = new UEvent();
        
        protected override void Awake()
        {
            base.Awake();
            var options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < GameManager.GameData.Levels.Count; i++)
            {
                if (GameManager.GameData.Levels[i] != null)
                {
                    options.Add(new TMP_Dropdown.OptionData(GameManager.GameData.Levels[i].transform.name));
                }
            }
            dropdown.options = options;

            controller = GetComponentInParent<UIController>();
        }

        public void LoadLevel()
        {
            GameManager.GameData.Saves.LevelData.SetLevel(dropdown.value);
            GameManager.GameData.Saves.Save();
            
            controller.NextLevel();
        }

        public void Add()
        {
            GameManager.GameData.Saves.PlayerData.IncreaseMoney(int.Parse("0" + moneyInput.text));
            GameManager.GameData.Saves.Save();
        }

        public void Win()
        {
            controller.Win();
            HideAll.Run();
        }

        public void Lose()
        {
            controller.Loose();
            HideAll.Run();
        }

        public void NextLevel()
        {
            controller.NextLevel();
        }
    }
}
