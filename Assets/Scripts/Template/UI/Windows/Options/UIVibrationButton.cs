using System;
using Template.Managers;

namespace Template.UI.Windows
{
    public class UIVibrationButton : UIOptionsButton
    {
        private void OnEnable()
        {
            active = GameManager.GameData.Saves.options.vibration;
            Active();
        }

        public override void Active()
        {
            base.Active();
            GameManager.GameData.Saves.options.vibration = active;
            GameManager.GameData.Saves.Save();
        }
    }
}
