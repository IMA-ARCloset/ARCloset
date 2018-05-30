using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgyptController : MonoBehaviour
{

    public Manager manager;
    public GameObject[] l_EgyptTorchs;
    public GameObject[] l_waterfalls;

    // Use this for initialization
    void Awake()
    {
        l_EgyptTorchs = GameObject.FindGameObjectsWithTag("EgyptTorch");

        l_waterfalls = GameObject.FindGameObjectsWithTag("Waterfalls");
    }

    // Update is called once per frame
    void Update()
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

    public IEnumerator Manage_EgyptTorchs()
    {

        if (manager.day)
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                t.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                t.GetComponentInChildren<ParticleSystem>().Play(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public IEnumerator Egypt_specialEffect()
    {
        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Play(true);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        manager.special_effectCorroutine = null;
    }
}
