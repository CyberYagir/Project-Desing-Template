using System;
using UnityEngine;

namespace Template
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
