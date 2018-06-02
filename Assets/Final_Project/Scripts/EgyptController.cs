using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgyptController : MonoBehaviour
{

    public Manager manager;
    public GameObject[] l_EgyptTorchs;
    public GameObject[] l_waterfalls;

    private Queue<AudioSource> l_waterfallsSounds;
    private Queue<AudioSource> l_torchsSounds;

    // Use this for initialization
    void Awake()
    {
        l_EgyptTorchs = GameObject.FindGameObjectsWithTag("EgyptTorch");

        l_waterfalls = GameObject.FindGameObjectsWithTag("Waterfalls");

        l_waterfallsSounds = new Queue<AudioSource>();
        l_torchsSounds = new Queue<AudioSource>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        if (manager.day)
            foreach (GameObject gO in l_EgyptTorchs)
            {
                gO.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public IEnumerator Manage_EgyptTorchs(AudioManager am)
    {

        if (manager.day)
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                AudioSource aS = l_torchsSounds.Dequeue();
                if (aS != null)
                    Debug.LogWarning("UN SONIDO ES NULL");
                //aS.Stop();
                t.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);

                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                t.GetComponentInChildren<ParticleSystem>().Play(true);
                AudioSource aS = am.PlayShoot("Torch");
                am.SetPitch(aS, Random.Range(0.8f, 1.2f));
                l_torchsSounds.Enqueue(aS);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public IEnumerator Egypt_specialEffect(AudioManager am)
    {
        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
            {
                p.Play(true);
                l_waterfallsSounds.Enqueue(am.PlayShoot("Waterfall"));
            }

        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
            {
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                AudioSource aS = l_waterfallsSounds.Dequeue();
                aS.Stop();
            }

        }

        manager.special_effectCorroutine = null;
    }
}
