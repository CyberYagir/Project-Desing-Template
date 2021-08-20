using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    Color color, glow;
    Renderer renderer;
    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }
    public void Set(Color c, Color g)
    {
        color = c; glow = g;
    }
    public void Update()
    {
        if (renderer)
        {
            renderer.material.color = Color.Lerp(renderer.material.color, color, 2 * Time.deltaTime);
            //renderer.material.SetColor("_EmissionColor", Color.Lerp(renderer.material.color, renderer.material.GetColor("_EmissionColor"), 2 * Time.deltaTime));
        }
    }
}
