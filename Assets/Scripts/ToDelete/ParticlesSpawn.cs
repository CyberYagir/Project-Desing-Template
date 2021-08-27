using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSpawn : MonoBehaviour
{
    List<GameObject> particles = new List<GameObject>();
    GameObject holder;
    public GameObject prefab;
    public float particleSize = 0.3f;
    private void Start()
    {
        holder = new GameObject();
        holder.name = "Holder Particles: " + transform.GetInstanceID();
        for (int i = 0; i < 4; i++)
        {
            particles.Add(Instantiate(prefab, holder.transform));
        }
    }

    private void OnDestroy()
    {
        Destroy(holder.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in collision.contacts)
        {
            if (collision.collider.GetComponent<Renderer>())
            {
                if (particles.Count != 0)
                {
                    var parts = particles[0];
                    particles.RemoveAt(0);


                    parts.transform.position = item.point + new Vector3(0, 0.5f, 0);
                    parts.transform.rotation = Quaternion.Euler(item.normal) * Quaternion.Euler(90, 0, 0);

                    float size = particleSize * transform.localScale.x;
                    parts.transform.localScale = new Vector3(size, size, size);
                    parts.GetComponent<ParticleSystemRenderer>().material.color = collision.collider.GetComponent<Renderer>().material.color / 1.1f;
                    //parts.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", collision.collider.GetComponent<Renderer>().material.GetColor("_EmissionColor") / 1.1f);
                    parts.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(BackToPool(parts));
                }
            }
        }
    }

    IEnumerator BackToPool(GameObject particle)
    {
        yield return new WaitForSeconds(1);
        particles.Add(particle);
    }
}
