using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathUI, winUI;


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Loose();
        }
    }

#endif

    public void StopGamePlay()
    {
        //Выключение игрока и др.
    }

    public void Win()
    {
        StopGamePlay();
        winUI.SetActive(true);
    }

    public void NextLevel()
    {
        GameManger.NextLevel();
    }

    public void Restart()
    {
        GameManger.Restart();
    }

    public void Loose()
    {
        StopGamePlay();
        deathUI.SetActive(true);
    }
}
