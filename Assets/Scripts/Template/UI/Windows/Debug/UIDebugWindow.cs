using System.Collections.Generic;
using Template.Managers;
using TMPro;
using UnityEngine;

namespace Template.UI.Windows
{
    public class UIDebugWindow : MonoCustom
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TMP_InputField moneyInput;
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
            GameManager.GameData.Saves.level = dropdown.value;
            GameManager.GameData.Saves.Save();
            UIManager.Instance.NextLevel();
        }

        public void Add()
        {
            GameManager.GameData.Saves.points += int.Parse("0" + moneyInput.text);
            GameManager.GameData.Saves.Save();
        }

        public void Win()
        {
            UIManager.Instance.Win();
        }

        public void Lose()
        {
            UIManager.Instance.Loose();
        }
    }
}
