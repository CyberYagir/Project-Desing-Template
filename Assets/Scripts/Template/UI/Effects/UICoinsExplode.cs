using System;
using System.Collections;
using Template.Managers;
using Template.UI.Overlays;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Template.UI.Effects
{
    public class UICoinsExplode : MonoCustom
    {
        [SerializeField] private UIParticleSystem particles;
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private float speed;
        [SerializeField] private float timeToGravity;

        private ParticleSystem.Particle[] coins = new ParticleSystem.Particle[200];

        public void Explode(Transform point, Action<int> OnCoinAdded, Action OnCoinsEnd, int delay = 2)
        {
            StartCoroutine(ExplodeAnimation(delay, point, OnCoinAdded, OnCoinsEnd));
        }

        private IEnumerator ExplodeAnimation(int delay, Transform point, Action<int> OnCoinAdded, Action OnCoinsEnd)
        {
            yield return new WaitForSeconds(delay);
            
            particles.enabled = true;
            particleSystem.Play();
            
            yield return new WaitForSeconds(timeToGravity);


            particleSystem.GetParticles(coins);
            int count = 0;
            while (true)
            {
                MoveParticles(ref count, point, OnCoinAdded);
                
                particleSystem.SetParticles(coins);
                if (count >= coins.Length)
                {
                    OnCoinsEnd.Invoke();
                    yield break;
                }

                yield return null;
            }
        }

        private void MoveParticles(ref int count, Transform point, Action<int> OnCoinAdded)
        {
            for (int i = 0; i < coins.Length; i++)
            {
                if (coins[i].remainingLifetime != 0)
                {
                    Vector3 v1 = particleSystem.transform.TransformPoint(coins[i].position);
                    Vector3 v2 = point.transform.position;

                    v1 = Vector3.MoveTowards(v1, v2, (coins[i].startLifetime / particleSystem.main.startLifetime.constant) * speed * Time.deltaTime);

                    if ((v1 - v2).sqrMagnitude < 0.01f)
                    {
                        coins[i].remainingLifetime = 0;
                        count++;
                        OnCoinAdded.Invoke(coins.Length);
                    }

                    coins[i].position = particleSystem.transform.InverseTransformPoint(v1);
                }
            }
        }
    }
}
