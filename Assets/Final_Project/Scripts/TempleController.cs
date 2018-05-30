using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempleController : MonoBehaviour {

    public Manager manager;
    public GameObject[] l_TempleTorchs;
    public GameObject[] l_flamethrower;

    // Use this for initialization
    void Awake () {
        l_TempleTorchs = GameObject.FindGameObjectsWithTag("Torch");
        l_TempleTorchs = l_TempleTorchs.OrderBy(go => go.name).ToArray();

        l_flamethrower = GameObject.FindGameObjectsWithTag("FlameThrower");
        l_flamethrower = l_flamethrower.OrderBy(go => go.name).ToArray();
    }

    void OnEnable()
    {
        if(manager.day)
            foreach (GameObject gO in l_TempleTorchs)
            {
                gO.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

        foreach (GameObject gO in l_flamethrower)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public IEnumerator Manage_templeTorchs()
    {

        if (manager.day)
        {
            for (int i = 0; i < 6; i += 2)
            {
                l_TempleTorchs[i].GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting); //Stop(true, ParticleSystemStopBehavior.StopEmitting);
                l_TempleTorchs[i + 1].GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                yield return new WaitForSeconds(1.5f);
            }
        }
        else
        {
            for (int i = 0; i < 6; i += 2)
            {
                l_TempleTorchs[i].GetComponentInChildren<ParticleSystem>().Play(true); //Stop(true, ParticleSystemStopBehavior.StopEmitting);
                l_TempleTorchs[i + 1].GetComponentInChildren<ParticleSystem>().Play(true);
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    public IEnumerator Temple_specialEffect()
    {
        foreach (GameObject gO in l_flamethrower)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Play(true);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_flamethrower)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        manager.special_effectCorroutine = null;
    }
}
