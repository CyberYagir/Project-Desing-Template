using UnityEngine;

/// <summary>
/// Скрипт который находится на канвасе и управляет логикой UI
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// <b>Синглетон</b> менеджера для обращения к <b>НЕ</b> статическим методам и переменным. 
    /// </summary>
    public static UIManager instance;

    [SerializeField] GameObject deathUI, winUI;
    [SerializeField] GameObject tapToPlay;

    #region Mono
    private void Start()
    {
        instance = this;
        InitTapToPlay();
    }

    /// <summary>
    /// Скрытие текста TapToPlay при первом нажатии путём привязывания к эвенту. 
    /// </summary>
    public void InitTapToPlay()
    {
        if (tapToPlay != null)
        {
            if (GameManager.gameStage == GameStage.StartWait)
            {
                tapToPlay.SetActive(true);
                GameManager.TapToPlayUI += () => { Tweaks.AnimationPlayType(tapToPlay, PlayType.Rewind); }; //Анимации к эвенту тапа
            }
            else
            {
                tapToPlay.SetActive(false);
            }
        }
    }

    private void Update()
    {
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
#endif
    }

    /// <summary>
    /// Вызов <b>NextLevel</b> у GameManager
    /// </summary>
    public void NextLevel()
    {
        GameManager.NextLevel();
    }

    /// <summary>
    /// Вызов <b>Restart</b> у GameManager
    /// </summary>
    public void Restart()
    {
        GameManager.Restart();
    }

    #endregion

    #region Evens_Win_Loose
    /// <summary>
    /// Метод победы. Вызырает действия связанные с обработкой победы и UI.
    /// </summary>
    public void Win()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManager.OnLevelEnd();
            winUI.SetActive(true);
        }
    }

    /// <summary>
    /// Метод проигрыша. Вызырает действия связанные с обработкой победы и UI.
    /// </summary>
    public void Loose()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManager.OnLevelEnd(false);
            deathUI.SetActive(true);
        }
    }

    #endregion
}
