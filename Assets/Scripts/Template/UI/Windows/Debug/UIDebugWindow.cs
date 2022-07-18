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
        public UEvent HideAll = new UEvent();
        protected override void Awake()
        {
            base.Awake();
            var options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < GameManager.GameData.MainData.levelList.Count; i++)
            {
                if (GameManager.GameData.MainData.levelList[i] != null)
                {
                    options.Add(new TMP_Dropdown.OptionData(GameManager.GameData.MainData.levelList[i].transform.name));
                }
            }
            dropdown.options = options;
        }

        public void LoadLevel()
        {
            GameManager.GameData.Saves.LevelData.SetLevel(dropdown.value);
            GameManager.GameData.Saves.Save();
            UIManager.Instance.NextLevel();
        }

        public void Add()
        {
            GameManager.GameData.Saves.PlayerData.IncreaseMoney(int.Parse("0" + moneyInput.text));
            GameManager.GameData.Saves.Save();
        }

        public void Win()
        {
            UIManager.Instance.Win();
            HideAll.Run();
        }

        public void Lose()
        {
            UIManager.Instance.Loose();
            HideAll.Run();
        }

        public void NextLevel()
        {
            UIManager.Instance.NextLevel();
        }
    }
}