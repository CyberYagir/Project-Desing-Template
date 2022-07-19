using System.Collections;
using DG.Tweening;
using Template.Managers;
using Template.UI.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Template.UI.Overlays
{
    public class UIWinMoney : MonoCustom
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private UICoinsExplode explode;
        [SerializeField] private DOTweenAnimation showAnimation;
        
        public const int Reward = 500;

        private int startMoney;
        
        public void Init()
        {
            startMoney = GameManager.GameData.Saves.PlayerData.Coins;
            showAnimation.onComplete.AddListener(ShowAnim);
            showAnimation.DOPlay();
            text.text = NonAllocString.instance + " + " + UIMoneyDisplay.FormatNumber(Reward) + " <sprite=15>";
        }


        public void ShowAnim()
        {
            GameManager.GameData.Saves.PlayerData.IncreaseMoney(Reward);
            UIMoneyDisplay.Instance.SetText(startMoney);
            explode.Explode(UIMoneyDisplay.Instance.CoinPoint, OnMoneyAdded, OnMoneyEnd, (int)showAnimation.duration * 2);
        }


        public void OnMoneyAdded(int count)
        {
            startMoney += Reward / count;
            UIMoneyDisplay.Instance.SetText(startMoney);
        }
        public void OnMoneyEnd()
        {
            UIMoneyDisplay.Instance.SetText(GameManager.GameData.Saves.PlayerData.Coins);
        }
    }
}
