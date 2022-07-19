using Template.Managers;
using Template.UI.Windows;
using UnityEditor;
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
                var controller = GetComponentInParent<UIController>();
                var debugW = Instantiate(debugWindow.gameObject, controller.transform).GetComponent<WindowAnimations>();
                debugW.GetCanvas().sortingLayerID = 1;
                debugW.Init(controller);
                debugW.GetComponent<UIDebugWindow>().HideAll.AddListener(HideOptions);
                button.onClick.AddListener(debugW.ShowWindow);
            }
        }

        public void HideOptions()
        {
            GetComponentInParent<WindowAnimations>().HideWindow(false);
        }
    }
}
