using System;
using UnityEngine;

namespace Template
{
    public class CameraFollow : MonoCustom
    {
        public static CameraFollow Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
