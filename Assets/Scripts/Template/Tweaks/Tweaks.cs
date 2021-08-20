using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweaks : MonoBehaviour
{
    #region Animations
    /// <summary>
    /// Animator/Animation
    /// </summary>
    public static void PlayAnim(GameObject obj, string animName)
    {
        PlayAnim(obj.transform, animName);
    }

    /// <summary>
    /// Animator/Animation
    /// </summary>
    public static void PlayAnim(Component obj, string animName)
    {
        var animation = obj.GetComponent<Animation>();
        var animator = obj.GetComponent<Animator>();
        if (animation)
        {
                animation.Play(animName);
        }
        else if (animator)
        {
            animator.Play(animName);
        }
        else
            Debug.LogError($"Yaroslav: Animation/Animator not found. Anim not played. Sender {obj.gameObject.name}");
    }

    /// <summary>
    /// Animation only
    /// </summary>
    public static void PlayAnim(GameObject obj, int animName)
    {
        PlayAnim(obj.transform, animName);
    }
    /// <summary>
    /// Animation only
    /// </summary>
    public static void PlayAnim(Component obj, int id)
    {
        var animation = obj.GetComponent<Animation>();
        if (animation)
        {
            var ideach = 0;
            foreach (AnimationState item in animation)
            {
                if (id == ideach)
                {
                    animation.Play(item.name);
                    break;
                }
                ideach++;
            }
        }
        else
            Debug.LogError($"Yaroslav: Animation/Animator not found. Anim not played. Sender {obj.gameObject.name}");
    }

    #endregion

    #region UIAnimations

    /// <summary>
    /// UIAnimate
    /// </summary>
    public static void AnimationPlayType(GameObject obj, PlayType playType)
    {
        AnimationPlayType(obj.transform, playType);
    }


    /// <summary>
    /// UIAnimate
    /// </summary>
    public static void AnimationPlayType(Component obj, PlayType playType)
    {
        var animate = obj.GetComponent<UIAnimate>();
        if (animate)
        {
            animate.playType = playType;
        }
        else
        {
            Debug.LogError($"Yaroslav: UIAnimate not found. Sender {obj.gameObject.name}");
        }
    }
    #endregion
}
