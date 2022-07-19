using DG.Tweening;
using Template.Managers;
using TMPro;
using UnityEngine;

namespace Template.UI.Overlays
{
    public class UIMoneyDisplay : Singleton<UIMoneyDisplay>
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Transform magnetPoint;
        private bool canPunch;

        public Transform CoinPoint => magnetPoint;

        protected void Awake()
        {
            SingletonSet(this);   
            GameManager.GameData.Saves.PlayerData.OnIncreaseMoney += UpdateText;
            UpdateText();
        }

        public void UpdateText()
        {
            text.text = FormatNumber(GameManager.GameData.Saves.PlayerData.Coins);
            Punch();     
        }

        public void Punch()
        {
            if (!canPunch)
            {
                transform.DOKill();
                transform.localScale = Vector3.one;
                transform.DOPunchScale(Vector3.one * 0.2f, 0.1f).SetLink(gameObject).onComplete += () => { canPunch = false;};
                canPunch = true;
            }
        }
        
        public void SetText(int val)
        {
            text.text = FormatNumber(val);
            Punch();
        }

        public static string FormatNumber(int num) 
        {
            if (num >= 100000000) {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000) {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000) {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000) {
                return (num / 1000D).ToString("0.##k");
            }
            return num.ToString("#,0");
        }
    }
}
