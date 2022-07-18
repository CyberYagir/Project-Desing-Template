using DG.Tweening;
using Template.Tweaks;
using Template.UI;
using Template.UI.Windows;
using TMPro;
using UnityEngine;

namespace Template.Managers
{
    /// <summary>
    /// Скрипт который находится на канвасе и управляет логикой UI
    /// </summary>
    public class UIManager : MonoCustom
    {
        [System.Serializable]
        public class Overlays
        {
            public UIAnimatedOverlay loseOverlay, winOverlay;
            public bool IsShowed => loseOverlay.IsShowed || winOverlay.IsShowed;
        }
        
        /// <summary>
        /// <b>Синглетон</b> менеджера для обращения к <b>НЕ</b> статическим методам и переменным. 
        /// </summary>
        public static UIManager Instance;

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Overlays overlays;
        [SerializeField] private UICrossSceneFader crossFader;
        [SerializeField] DOTweenAnimation tapToPlay;

        #region Mono

        public override void OnStart()
        {
            base.OnStart();
            Instance = this;
            levelText.text = NonAllocString.instance + $"Level {GameManager.GameData.Saves.LevelData.CompletedCount}";
            InitTapToPlay();
            
        }

        /// <summary>
        /// Скрытие текста TapToPlay при первом нажатии путём привязывания к эвенту. 
        /// </summary>
        public void InitTapToPlay()
        {
            if (tapToPlay != null)
            {
                if (GameManager.GameStage == GameStage.StartWait)
                {
                    tapToPlay.gameObject.SetActive(true);
                    tapToPlay.DOPlay();
                    GameManager.TapToPlayUI += () =>
                    {
                        tapToPlay.DOPlayBackwards();
                    }; //Анимации к эвенту тапа
                }
                else
                {
                    tapToPlay.gameObject.SetActive(false);
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            EditorControls();
        }
        #endregion

        #region Buttons

        /// <summary>
        /// Метод обрабатывает хоткеи во время игры в эдиторе.
        /// </summary>
        public void EditorControls()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F))
            {
                Win();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Loose();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLevel();
            }
            
#endif
        }

        /// <summary>
        /// Вызов <b>NextLevel</b> у GameManager
        /// </summary>
        public void NextLevel()
        {
            GameManager.NextLevel();
            crossFader.LoadScene(NonAllocString.instance + "Game");
        }

        /// <summary>
        /// Вызов <b>Restart</b> у GameManager
        /// </summary>
        public void Restart()
        {
            GameManager.Restart();
            crossFader.LoadScene("Game");
        }

        #endregion

        #region Evens_Win_Loose
        /// <summary>
        /// Метод победы. Вызырает действия связанные с обработкой победы и UI.
        /// </summary>
        public void Win()
        {
            if (!overlays.IsShowed)
            {
                GameManager.OnLevelEnd();
                overlays.winOverlay.Show();
            }
        }

        /// <summary>
        /// Метод проигрыша. Вызырает действия связанные с обработкой победы и UI.
        /// </summary>
        public void Loose()
        {
            if (!overlays.IsShowed)
            {
                GameManager.OnLevelEnd(false);
                overlays.loseOverlay.Show();
            }
        }

        #endregion
    }
}
