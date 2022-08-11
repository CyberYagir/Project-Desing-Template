using DG.Tweening;
using UnityEngine;

namespace Template.Tweaks
{
    public class Shaker
    {
        [System.Serializable]
        public class ShakeOptions
        {
            public int shakeLayer;
            public float duration;
            public float strength = 1f;
            public int vibrato = 10;
            public float randomness = 90f;
            public bool snapping = false;
            public bool fadeOut = true;
        }

        public static Shaker Instance { get; private set; }

        [ReadOnly] [SerializeField] private Transform transform;
        [ReadOnly] [SerializeField] private int currentShakeLayer;


        private Tweener tweener;

        public Shaker(Transform cameraObject)
        {
            Instance = this;
            transform = cameraObject;
        }

        public void Init(Transform cameraObject)
        {
            Instance = this;
            transform = cameraObject;
        }

        public void Shake(ShakeOptions shake)
        {
            if (shake.shakeLayer >= Instance.currentShakeLayer)
            {
                var camera = Instance.transform;
                camera.localPosition = Vector3.zero;
                tweener.Kill();
                tweener = camera.DOShakePosition(shake.duration, shake.strength, shake.vibrato, shake.randomness, shake.snapping, shake.fadeOut);
                tweener.onComplete += () => Instance.currentShakeLayer = 0;
                tweener.Play();
                Instance.currentShakeLayer = shake.shakeLayer;
            }
        }
    }
}