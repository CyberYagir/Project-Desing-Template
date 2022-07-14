using Template.Managers;

namespace Template.UI.Windows
{
    public class UISoundButton : UIOptionsButton
    {
        private void OnEnable()
        {
            active = GameManager.GameData.Saves.options.sound;
            Active();
        }

        public override void Active()
        {
            base.Active();
            GameManager.GameData.Saves.options.sound = active;
            GameManager.GameData.Saves.Save();
        }
    }
}
