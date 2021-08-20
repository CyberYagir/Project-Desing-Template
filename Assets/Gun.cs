using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FinalTile))]
public class Gun : MonoBehaviour
{
    FinalTile finalTile;
    float time;
    [SerializeField] float cooldown;
    [SerializeField] ParticleSystem particleSystem;
    Boss boss;
    private void Start()
    {
        boss = FindObjectOfType<Boss>();
        finalTile = GetComponent<FinalTile>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (finalTile.isIn)
        {
            if (Stack.stack > 0)
            {
                if (Input.GetKey(KeyCode.Mouse0) && time >= cooldown)
                {
                    boss.hp--;
                    time = 0;
                    Stack.stack--; particleSystem.Play();

                    if (boss.hp == 0)
                    {
                        FindObjectOfType<UIManager>().Win();
                        return;
                    }
                }
            }
            else
            {
                if (boss.hp > 0)
                {
                    FindObjectOfType<UIManager>().Loose();
                }
                else
                {
                    FindObjectOfType<UIManager>().Win();
                }
            }

        }
    }
}