using System.Collections.Generic;
using Template.Managers;
using Template.Tweaks;
using Template.UI.Overlays;
using TMPro;
using UnityEngine;

namespace Template.UI.Windows
{
    public class UIDebugWindow : UIControllerElement
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TMP_InputField moneyInput;
 
        public UEvent HideAll = new UEvent();

        public override void Init(UIController controller)
        {
            base.Init(controller);
            var options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < controller.GameData.Levels.Count; i++)
            {
                if (controller.GameData.Levels[i] != null)
                {
                    options.Add(new TMP_Dropdown.OptionData(controller.GameData.Levels[i].transform.name));
                }
            }
            dropdown.options = options;


        }
        

        public void LoadLevel()
        {
            controller.GameData.Saves.LevelData.SetLevel(dropdown.value);
            controller.GameData.Saves.Save();
            
            controller.NextLevel();
        }

        public void Add()
        {
            controller.GameData.Saves.PlayerData.IncreaseMoney(int.Parse("0" + moneyInput.text));
            controller.GameData.Saves.Save();
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
