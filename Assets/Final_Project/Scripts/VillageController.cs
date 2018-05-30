using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
	public Manager manager;
    public GameObject[] l_VillageLights;
	public GameObject[] l_VillageRain;

    private void Awake()
    {
        l_VillageLights = GameObject.FindGameObjectsWithTag("VillageLight");
    }

    private void OnEnable()
    {
        foreach (GameObject gO in l_VillageLights)
        {
            gO.SetActive(false);
        }
    }

	public IEnumerator Manage_VillageLights()
    {
        if (manager.day)
        {
            foreach (GameObject t in l_VillageLights)
            {
                //t.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                t.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            foreach (GameObject t in l_VillageLights)
            {
                //t.GetComponentInChildren<ParticleSystem>().Play(true);
                t.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
}

    public IEnumerator Village_specialEffect()
    {
        foreach (GameObject gO in l_VillageRain)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Play(true);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_VillageRain)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        manager.special_effectCorroutine = null;
    }
}
