using System;
using Template.Managers;

namespace Template.UI.Windows
{
    public class UIVibrationButton : UIOptionsButton
    {
        private void OnEnable()
        {
            active = GameManager.GameData.Saves.OptionsData.Vibration;
            Active();
        }

        public override void Active()
        {
            base.Active();
            GameManager.GameData.Saves.OptionsData.SetVibration(active);
            GameManager.GameData.Saves.Save();
        }
    }
}
