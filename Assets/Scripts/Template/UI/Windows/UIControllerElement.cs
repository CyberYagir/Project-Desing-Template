using Template.Managers;
using UnityEngine;

namespace Template.UI.Windows
{
    public class UIControllerElement : MonoBehaviour
    {
        protected UIController controller;

        public UIController Controller => controller;
        
        public virtual void Init(UIController controller)
        {
            this.controller = controller;
        }
    }
}