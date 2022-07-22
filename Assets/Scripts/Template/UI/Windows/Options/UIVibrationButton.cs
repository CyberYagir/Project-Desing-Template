using System;
using Template.Managers;

namespace Template.UI.Windows
{
    public class UIVibrationButton : UIOptionsButton
    {
        public override void Init(UIController controller)
        {
            base.Init(controller);
            active = controller.GameData.Saves.OptionsData.Vibration;
            Active();

        }
        public override void OnStart()
        {
            base.OnStart();
        }

        public override void Active()
        {
            base.Active();
            controller.GameData.Saves.OptionsData.SetVibration(active);
            controller.GameData.Saves.Save();
        }
    }
}
