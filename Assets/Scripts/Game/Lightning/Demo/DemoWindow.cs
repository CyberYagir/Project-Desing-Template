
using Template.Managers;
using Template.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Lightning.Demo
{
    public class DemoWindow : UIControllerElement
    {
        [SerializeField] private CameraFogger camera;
        [SerializeField] private Transform rendered;
        [SerializeField] private RectTransform windowRender;


        private RenderTexture texture;
        public override void Init(UIController controller)
        {
            base.Init(controller);
            rendered.transform.parent = null;
            rendered.transform.localScale = Vector3.one;
            rendered.gameObject.SetActive(false);

            texture = new RenderTexture((int)windowRender.rect.width, (int)windowRender.rect.height, 16);


            windowRender.GetComponent<RawImage>().texture = texture;

        }

        public void Open()
        {
            gameObject.SetActive(true);
            camera.ActivateCamera(true);
            rendered.gameObject.SetActive(true);
            camera.Init();
            camera.Camera.targetTexture = texture;
        }

        public void Close()
        {
            camera.ActiveCamera.ActivateCamera();
            rendered.gameObject.SetActive(false);
            gameObject.SetActive(false);

        }
    }
}
