using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Template.Tweaks
{
    [System.Serializable]
    public class Event : UnityEvent
    {
        public static Event operator+ (Event b, UnityAction c) {
            b.AddListener(c);
            return b;
        }
        public static Event operator- (Event b, UnityAction c) {
            b.RemoveListener(c);
            return b;
        }

        public void Run()
        {
            this.Invoke();
        }
    }

    public class Event<T> : UnityEvent<T>
    {
        public static Event<T> operator+ (Event<T> b, UnityAction<T> c) {
            b.AddListener(c);
            return b;
        }
        public static Event<T> operator- (Event<T>  b, UnityAction<T> c) {
            b.RemoveListener(c);
            return b;
        }
    
        public void Run(T data)
        {
            this.Invoke(data);
        }
    }
}