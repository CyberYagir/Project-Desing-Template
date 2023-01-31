using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Lightning.Scriptable
{
    [CreateAssetMenu(fileName = "LocalLightning", menuName = "Yaroslav/LocalLightning", order = 1)]
    public sealed class LocalLightningObject : ScriptableObject
    {
        [System.Serializable]
        public sealed class Range
        {
            [SerializeField] private float min, max;

            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }

            public float Min => min;
            public float Max => max;
        }

        [System.Serializable]
        public sealed class Fog
        {
            [SerializeField] private bool haveFog;
            [SerializeField] private FogMode fogMode = FogMode.Linear;
            [SerializeField] private Color fogColor;
            [Header("Linear")] [SerializeField] private Range fogRange;

            [Header("Exponential")] [SerializeField]
            private float fogDestiny;

            public static Fog GetFromScene()
            {
                var fog = new Fog();

                fog.haveFog = RenderSettings.fog;
                fog.fogMode = RenderSettings.fogMode;
                fog.fogColor = RenderSettings.fogColor;
                fog.fogRange = new Range(RenderSettings.fogStartDistance, RenderSettings.fogEndDistance);
                fog.fogDestiny = RenderSettings.fogDensity;

                return fog;
            }

            public void LoadData()
            {
                RenderSettings.fog = haveFog;
                RenderSettings.fogMode = fogMode;
                RenderSettings.fogColor = fogColor;
                RenderSettings.fogStartDistance = fogRange.Min;
                RenderSettings.fogEndDistance = fogRange.Max;
                RenderSettings.fogDensity = fogDestiny;
            }
        }

        [System.Serializable]
        public sealed class Lightning
        {
            [System.Serializable]
            public class LightFlat
            {
                [SerializeField, ColorUsage(false, true)]
                private Color ambientLight;

                public LightFlat(Color ambientLight)
                {
                    this.ambientLight = ambientLight;
                }

                public void LoadData()
                {
                    RenderSettings.ambientLight = ambientLight;
                }
            }

            [System.Serializable]
            public class LightTrilight
            {
                [SerializeField, ColorUsage(false, true)]
                private Color ambientSky;

                [SerializeField, ColorUsage(false, true)]
                private Color ambientHorizon;

                [SerializeField, ColorUsage(false, true)]
                private Color ambientGround;

                public LightTrilight(Color ambientSky, Color ambientHorizon, Color ambientGround)
                {
                    this.ambientSky = ambientSky;
                    this.ambientHorizon = ambientHorizon;
                    this.ambientGround = ambientGround;
                }

                public void LoadData()
                {
                    RenderSettings.ambientSkyColor = ambientSky;
                    RenderSettings.ambientEquatorColor = ambientHorizon;
                    RenderSettings.ambientGroundColor = ambientGround;
                }
            }

            [System.Serializable]
            public class LightSkybox
            {
                [SerializeField] private float intensivity = 1;

                public LightSkybox(float intensivity)
                {
                    this.intensivity = intensivity;
                }

                public void LoadData()
                {
                    RenderSettings.ambientIntensity = intensivity;
                }
            }

            [System.Serializable]
            public class SunData
            {
                [SerializeField] private bool haveSunData;
                [SerializeField] private Vector3 sunRotation;

                public SunData(Light sun)
                {
                    haveSunData = sun != null;
                    if (haveSunData)
                    {
                        sunRotation = sun.transform.eulerAngles;
                    }
                }

                public void LoadData(Light sun)
                {
                    if (sun != null && haveSunData)
                    {
                        sun.transform.eulerAngles = sunRotation;
                    }
                }
            }

            [SerializeField] private Color shadowColor;
            [SerializeField] private Material skyBox;
            [SerializeField] private SunData sunData;
            [SerializeField] private AmbientMode ambientMode = AmbientMode.Flat;
            [Space] [SerializeField] private LightFlat colorAmbient;
            [SerializeField] private LightTrilight gradientAmbient;
            [SerializeField] private LightSkybox skyBoxAmbient;



            public void LoadData(Light sun)
            {
                RenderSettings.subtractiveShadowColor = shadowColor;
                RenderSettings.skybox = skyBox;
                sunData.LoadData(sun);
                RenderSettings.ambientMode = ambientMode;
                colorAmbient.LoadData();
                gradientAmbient.LoadData();
                skyBoxAmbient.LoadData();
            }

            public static Lightning GetFromScene(Light sun)
            {
                var lig = new Lightning();

                lig.shadowColor = RenderSettings.subtractiveShadowColor;
                lig.skyBox = RenderSettings.skybox;
                lig.sunData = new SunData(sun);
                lig.ambientMode = RenderSettings.ambientMode;
                lig.colorAmbient = new LightFlat(RenderSettings.ambientLight);
                lig.gradientAmbient = new LightTrilight(RenderSettings.ambientSkyColor, RenderSettings.ambientEquatorColor, RenderSettings.ambientGroundColor);
                lig.skyBoxAmbient = new LightSkybox(RenderSettings.ambientIntensity);


                return lig;
            }
        }

        [SerializeField] private Fog fogOptions;
        [SerializeField] private Lightning lightningOptions;


        public void GetDataFromScene(Light sun)
        {
            fogOptions = Fog.GetFromScene();
            lightningOptions = Lightning.GetFromScene(sun);
        }

        public void LoadDataToScene(Light sun)
        {
            fogOptions.LoadData();
            lightningOptions.LoadData(sun);
        }
    }
}
