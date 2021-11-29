using DG.Tweening;
using UnityEngine;

namespace Template.Tweaks
{
    public class UIAnimate : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation forward, backward;

        public void PlayForward()
        {
            forward.DOPlay();
        }
        public void PlayBackward()
        {
            backward.DOPlay();
        }
    }
}
