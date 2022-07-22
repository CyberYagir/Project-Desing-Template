using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.UI
{
    public class UIControllerElement : MonoCustom
    {
        [SerializeField] private List<UIControllerElement> subElements;
        protected UIController controller;
        public UIController Controller => controller;
        
        
        
        public virtual void Init(UIController controller)
        {
            for (int i = 0; i < subElements.Count; i++)
            {
                if (subElements[i] != null && subElements[i] != this)
                {
                    subElements[i].Init(controller);
                }
            }
            this.controller = controller;
        }
    }
}