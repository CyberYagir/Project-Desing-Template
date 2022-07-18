using UnityEngine;

namespace Template.Managers
{
    public class HapticManager : MonoBehaviour
    {
        public static HapticManager Instance;
        private float lastTimeHaptic;
        private void Awake()
        {
            Instance = this;
            Taptic.tapticOn = true;
        }

        public static void PlayHaptic()
        {
            if(!GameManager.GameData.Saves.OptionsData.Vibration)
                return;
        
            if (Time.time - Instance.lastTimeHaptic < 0.05f)
                return;
        
            //MMVibrationManager.Haptic (HapticTypes.LightImpact);
        
            Taptic.Light();
        
            Instance.lastTimeHaptic = Time.time;

#if UNITY_EDITOR
            Debug.Log("VIBRATE");
#endif
        }
    }
}
