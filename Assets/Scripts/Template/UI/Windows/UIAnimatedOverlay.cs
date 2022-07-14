using UnityEngine;

namespace Template.UI.Windows
{
    public class UIAnimatedOverlay : MonoCustom
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Animation animation;
        public bool IsShowed => canvas.enabled;

        public override void OnStart()
        {
            base.OnStart();
            canvas.enabled = false;
            animation.enabled = false;
        }


        public void Show()
        {
            canvas.enabled = true;
            animation.enabled = true;
            animation.Stop();
            animation.PlayQueued(animation.clip.name);
        }
        
        public void ShowAnimation(string animationName)
        {
            canvas.enabled = true;
            animation.enabled = true;
            animation.Stop();
            animation.PlayQueued(animationName);
        }
    }
}
