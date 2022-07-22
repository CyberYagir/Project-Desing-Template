using DG.Tweening;
using Template.Scriptable;
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
    public class UIController : MonoCustom
    {
        [System.Serializable]
        public class Overlays
        {
            public UIAnimatedOverlay loseOverlay, winOverlay;
            public bool IsShowed => loseOverlay.IsShowed || winOverlay.IsShowed;
        }

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private UICrossSceneFader crossFader;
        [Space]
        [SerializeField] private ElementsController elementsController;
        [SerializeField] private Overlays overlays;

        [Header("Data")]
        [ReadOnly] [SerializeField] private LevelLogic levelLogic;
        [ReadOnly] [SerializeField] private GameManager gameManager;


        public bool IsOverUI => elementsController.IsOverElements();
        
        public GamePhase GamePhase
        {
            get => levelLogic.GamePhase;
            set => levelLogic.ChangePhase(value);
        }
        public GameDataObject GameData => gameManager?.GameData;
        

        #region Mono

        public void Init(LevelLogic level, GameManager gameManager)
        {
            levelLogic = level;
            this.gameManager = gameManager;
            elementsController.Init(this);
        }
        
        public override void OnStart()
        {
            base.OnStart();
            levelText.text = NonAllocString.instance + $"Level {gameManager.GameData.Saves.LevelData.CompletedCount}";
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
            GameManager.NextLevel(gameManager.GameData);
            crossFader.LoadScene(NonAllocString.instance + "Game");
        }

        /// <summary>
        /// Вызов <b>Restart</b> у GameManager
        /// </summary>
        public void Restart()
        {
            GameManager.Restart(gameManager.GameData);
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
                gameManager.OnLevelEnd();
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
                gameManager.OnLevelEnd(false);
                overlays.loseOverlay.Show();
            }
        }

        #endregion
    }
}
