using System.Collections.Generic;
using Template.Managers;
using UnityEngine;

namespace Template.Scriptable
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "Yaroslav/Sounds Data", order = 2)]
    public class SoundDataObject : ScriptableObject
    {
        [System.Serializable]
        public class Sound
        {
            [SerializeField]
            private List<AudioClip> clips;

            public AudioClip GetRandomClip()
            {
                if (clips.Count == 0) return null;
                if (!GameManager.GameData.Saves.OptionsData.Sound) return null;
                return clips[Random.Range(0, clips.Count)];
            }

            public AudioClip this[int index]
            {
                get { return clips[index]; }

                set { clips[index] = value; }
            }
        }
        public Sound testSound;
    }
}
