using System;
using Game.Lightning.Scriptable;
using Template;
using UnityEngine;

namespace Game.Lightning
{
    [RequireComponent(typeof(Camera))]
    public class CameraFogger : Singleton<CameraFogger>
    {
        [SerializeField] private LocalLightningObject localLightning;
        [SerializeField] private Light customSun;
        
        
        private Camera camera;
        public CameraFogger ActiveCamera => Instance;

        public Camera Camera => camera;


        public void Init()
        {
            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }

            if (ActiveCamera == null)
            {
                SingletonSet(this);
                ActivateCamera();
            }
        }
        
        public void ActivateCamera(bool dataOnly = false)
        {
            if (!dataOnly)
            {
                ActiveCamera.gameObject.SetActive(false);
                gameObject.SetActive(true);
                SingletonSet(this);
            }

            localLightning.LoadDataToScene(customSun == null ? RenderSettings.sun : customSun);
            
        }
    }
}
