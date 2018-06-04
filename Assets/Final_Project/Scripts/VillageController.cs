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
        l_VillageRain = GameObject.FindGameObjectsWithTag("Rain");

        /*
            Paramos los sonidos cuando se carga la escena
        */
        foreach (var gO in l_VillageRain)
            gO.GetComponent<AudioSource>().Stop();

        GetComponent<AudioSource>().Stop();
    }

    private void OnEnable()
    {
        if (manager.day)
        {
            GetComponent<AudioSource>().Stop();
            foreach (GameObject gO in l_VillageLights)
            {
                gO.SetActive(false);
            }
        }


        foreach (GameObject gO in l_VillageRain)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            
            gO.GetComponent<AudioSource>().Stop();
        }
    }

    public IEnumerator Manage_VillageLights()
    {
        AudioSource aS = GetComponent<AudioSource>();
        if (manager.day)
        {
            if (aS != null && aS.isPlaying)
                aS.Stop();

            foreach (GameObject t in l_VillageLights)
            {
                t.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            if (aS != null)
                aS.Play();

            foreach (GameObject t in l_VillageLights)
            {
                t.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public IEnumerator Village_specialEffect(AudioManager am)
    {
        AudioSource aS;
        foreach (GameObject gO in l_VillageRain)
        {
            aS = gO.GetComponent<AudioSource>();
            if (aS != null)
            {
                aS.pitch = Random.Range(0.8f, 1.2f);
                aS.Play();
            }
            else
            {
                Debug.LogWarning("No se ha encontrado AudioSource");
            }
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Play(true);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_VillageRain)
        {
            aS = gO.GetComponent<AudioSource>();
            if (aS != null && aS.isPlaying)
            {
                aS.Stop();
            }
            else
            {
                Debug.LogWarning("No se ha encontrado AudioSource");
            }
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        manager.special_effectCorroutine = null;
    }
}
