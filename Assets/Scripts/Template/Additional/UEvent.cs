using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Tweaks
{
    [System.Serializable]
    public class UEvent : UnityEvent
    {
        public static UEvent operator+ (UEvent b, UnityAction c) {
            b.AddListener(c);
            return b;
        }
        public static UEvent operator- (UEvent b, UnityAction c) {
            b.RemoveListener(c);
            return b;
        }

        public void Run()
        {
            this.Invoke();
        }
    }

    public class UEvent<T> : UnityEvent<T>
    {
        public static UEvent<T> operator+ (UEvent<T> b, UnityAction<T> c) {
            b.AddListener(c);
            return b;
        }
        public static UEvent<T> operator- (UEvent<T>  b, UnityAction<T> c) {
            b.RemoveListener(c);
            return b;
        }
    
        public void Run(T data)
        {
            this.Invoke(data);
        }
    }
}