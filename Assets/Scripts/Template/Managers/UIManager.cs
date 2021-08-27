using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject deathUI, winUI;
    [SerializeField] GameObject tapToPlay;

    #region Mono
    private void Start()
    {
        instance = this;
        InitTapToPlay();
    }

    public void InitTapToPlay()
    {
        if (tapToPlay != null)
        {
            if (GameManger.instance.gameStage == GameStage.StartWait)
            {
                tapToPlay.SetActive(true);
                GameManger.TapToPlayUI += () => { Tweaks.AnimationPlayType(tapToPlay, PlayType.Rewind); }; //Анимации к эвенту тапа
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

    public void NextLevel()
    {
        GameManger.NextLevel();
    }

    public void Restart()
    {
        GameManger.Restart();
    }

    #endregion

    #region Evens_Win_Loose
    public void Win()
    {
        GameManger.OnLevelEnd();
        winUI.SetActive(true);
    }

    public void Loose()
    {
        GameManger.OnLevelEnd(false);
        deathUI.SetActive(true);
    }

    #endregion
}
