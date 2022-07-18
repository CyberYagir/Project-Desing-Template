using Template.Managers;

namespace Template.UI.Windows
{
    public class UISoundButton : UIOptionsButton
    {
        private void OnEnable()
        {
            active = GameManager.GameData.Saves.OptionsData.Sound;
            Active();
        }

        public override void Active()
        {
            base.Active();
            GameManager.GameData.Saves.OptionsData.SetSound(active);
            GameManager.GameData.Saves.Save();
        }
    }
}
