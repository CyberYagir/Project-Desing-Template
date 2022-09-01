using Template.Managers;

namespace Template.UI.Windows
{
    public class UISoundButton : UIOptionsButton
    {

        public override void Init(UIController controller)
        {
            base.Init(controller);
            active = controller.GameData.Saves.OptionsData.Sound;
            Active();
        }

        public override void Active()
        {
            base.Active();
            controller.GameData.Saves.OptionsData.SetSound(active);
            controller.GameData.Saves.Save();
        }
    }
}
