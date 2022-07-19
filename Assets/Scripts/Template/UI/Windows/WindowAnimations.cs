using System.Collections;
using DG.Tweening;
using Template.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Template.UI.Windows
{
    public class WindowAnimations: UIControllerElement
    {
        [SerializeField] protected Image background;
        [SerializeField] protected Transform window;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float alpha = 0.9f, speed = 0.25f;
    
        public UnityEvent OnShow;
        public UnityEvent OnHide;

        [SerializeField] private bool isShowed;
        public bool IsShowed => isShowed;

        public virtual void Awake()
        {
            background.raycastTarget = false;
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
            window.transform.localScale = Vector3.zero;
            canvas.enabled = false;
        }

        private GamePhase phaseOnShow;

    
    
        public void ShowWindow()
        {
            if (!isShowed)
            {
                canvas.enabled = true;
                window.DOKill();
                background.DOKill();
                background.raycastTarget = true;
                background.DOFade(alpha, speed).SetLink(background.gameObject);
                window.DOScale(Vector3.one,speed).SetLink(window.gameObject);
                phaseOnShow = controller.GamePhase;
                controller.GamePhase = GamePhase.Pause;
                isShowed = true;
                OnShow.Invoke();
            }
        }

        public void HideWindow(bool unblock)
        {
            if (isShowed)
            {
                background.raycastTarget = false;
                background.DOFade(0, speed).SetLink(background.gameObject);
                window.DOScale(Vector3.zero, speed).SetLink(window.gameObject);
                print(phaseOnShow);
                controller.GamePhase = phaseOnShow;
                OnHide.Invoke();
                StopAllCoroutines();
                StartCoroutine(Wait(unblock));
            }
        }

        IEnumerator Wait(bool unblock)
        {

            yield return new WaitForSeconds(speed);

            if (unblock)
            {
                OnOffLocker(); //Unlock player controls
            }

            isShowed = false;
            canvas.enabled = false;
        }


        public virtual void OnOffLocker()
        {
        
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }
    }
}