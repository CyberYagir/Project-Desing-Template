using Template.Managers;
using Template.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Template.UI.Windows
{
    public class UIDebugButton : MonoCustom
    {
        [SerializeField] private WindowAnimations debugWindow;
        [SerializeField] private Button button;
        public override void OnStart()
        {
            base.OnStart();
            if (!GameManager.GameData.MainData.isDebugBuild)
            {
                gameObject.SetActive(false);
            }
            else
            {
                var debugW = Instantiate(debugWindow.gameObject, GameManager.Canvas.transform).GetComponent<WindowAnimations>();
                debugW.GetCanvas().sortingLayerID = 1;
                button.onClick.AddListener(debugW.ShowWindow);
            }
        }
    }
}
