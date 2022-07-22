using UnityEngine;
using UnityEngine.Events;

namespace Template.UI.Windows
{
    public class UIAnimatedOverlay : UIControllerElement
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private new Animation animation;
        [SerializeField] private UnityEvent OnShow;
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
            OnShow.Invoke();
        }
        
        public void ShowAnimation(string animationName)
        {
            canvas.enabled = true;
            animation.enabled = true;
            animation.Stop();
            animation.PlayQueued(animationName);
            OnShow.Invoke();
        }
    }
}
