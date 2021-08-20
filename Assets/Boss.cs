using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    int startHp;
    public int hp;
    [Space]
    [SerializeField] GameObject canvas;
    [SerializeField] Transform hpTrackPoint;
    [SerializeField] Transform hpCanvas, hpValue;


    private void Start()
    {
        startHp = hp; canvas.SetActive(true);
    }
    private void Update()
    {
        hpValue.transform.localScale = new Vector3((float)hp / (float)startHp, 1, 1);
        hpCanvas.transform.position = Camera.main.WorldToScreenPoint(hpTrackPoint.transform.position, Camera.MonoOrStereoscopicEye.Mono);
    }


}
