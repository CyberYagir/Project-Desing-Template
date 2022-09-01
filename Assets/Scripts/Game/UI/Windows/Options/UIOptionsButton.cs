using System;
using DG.Tweening;
using Template.UI.Overlays;
using UnityEngine;
using UnityEngine.UI;

namespace Template.UI.Windows
{
    public class UIOptionsButton : UIControllerElement
    {
        [SerializeField] private Sprite onSprite, offSprite;
        [SerializeField] private Image image;

        protected bool active;

        public void InvertActive()
        {
            active = !active;
            Active();
        }
        public virtual void Active()
        {
            image.sprite = active ? onSprite : offSprite;
            image.DOFade(active ? 1 : 0.5f, 0.2f).SetLink(image.gameObject);
        }
    }
}
