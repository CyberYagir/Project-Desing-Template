using System;
using System.Collections;
using DG.Tweening;
using Template.Managers;
using Template.UI.Windows;
using UnityEngine;

namespace Template.UI.Overlays
{
    public class UITapToPlay : UIControllerElement
    {
        [SerializeField] private DOTweenAnimation dotween;

        public override void Init(UIController controller)
        {
            base.Init(controller);
            if (gameObject.active)
            {
                dotween.DOPlay();
                controller.GamePhase = GamePhase.StartWait;
                StartCoroutine(WaitForTap());
            }
        }

        IEnumerator WaitForTap()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!controller.IsOverUI)
                    {
                        break;
                    }
                }
                yield return null;
            }
            controller.GamePhase = GamePhase.Game;
            dotween.DOPlayBackwards();
        }
    }
}
