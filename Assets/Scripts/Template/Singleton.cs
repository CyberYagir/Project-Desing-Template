using UnityEngine;

namespace Template
{
    public class Singleton<T>: MonoBehaviour
    {
        public static T Instance { get; private set; }

        public void SingletonSet(T obj)
        {
            Instance = obj;
        }
    }
    
}