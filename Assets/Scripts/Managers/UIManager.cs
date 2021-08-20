using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathUI, winUI;

    public void StopGamePlay()
    {
        FindObjectOfType<Gun>().enabled = false;
        FindObjectOfType<Player>().enabled = false;
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
