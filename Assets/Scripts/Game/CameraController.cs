using System;
using Template.Tweaks;
using UnityEngine;

namespace Template
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        private Shaker shaker;

        public void Init()
        {
            shaker = new Shaker(camera.transform);
        }
    }
}
