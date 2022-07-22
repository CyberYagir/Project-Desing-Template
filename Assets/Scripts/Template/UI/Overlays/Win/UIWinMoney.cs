using System.Collections;
using DG.Tweening;
using Template.Managers;
using Template.UI.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Template.UI.Overlays
{
    public class UIWinMoney : UIControllerElement
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private UICoinsExplode explode;
        [SerializeField] private DOTweenAnimation showAnimation;
        [SerializeField] private UIMoneyDisplay moneyDisplay;
        public const int Reward = 500;

        private int startMoney;
        
        public void Init()
        {
            startMoney = controller.GameData.Saves.PlayerData.Coins;
            showAnimation.onComplete.AddListener(ShowAnim);
            showAnimation.DOPlay();
            text.text = NonAllocString.instance + " + " + UIMoneyDisplay.FormatNumber(Reward) + " <sprite=15>";
        }


        public void ShowAnim()
        {
            if (moneyDisplay != null)
            {
                controller.GameData.Saves.PlayerData.IncreaseMoney(Reward);
                moneyDisplay.SetText(startMoney);
                explode.Explode(moneyDisplay.CoinPoint, OnMoneyAdded, OnMoneyEnd, (int) showAnimation.duration * 2);
            }
        }


        public void OnMoneyAdded(int count)
        {
            startMoney += Reward / count;
            moneyDisplay.SetText(startMoney);
        }
        public void OnMoneyEnd()
        {
            moneyDisplay.SetText(controller.GameData.Saves.PlayerData.Coins);
        }
    }
}
