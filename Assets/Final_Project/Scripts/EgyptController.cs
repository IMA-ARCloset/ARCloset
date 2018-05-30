using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgyptController : MonoBehaviour {

    public Manager manager;
    public GameObject[] l_EgyptTorchs;

    // Use this for initialization
    void Awake () {
        l_EgyptTorchs = GameObject.FindGameObjectsWithTag("EgyptTorch");
    }
	
	// Update is called once per frame
	void Update () {
        if (manager.day)
            foreach (GameObject gO in l_EgyptTorchs)
            {
                gO.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
    }

    public void manageTorchs()
    {
        StartCoroutine(Manage_EgyptTorchs());
    }

    IEnumerator Manage_EgyptTorchs()
    {

        if (manager.day)
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                t.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            foreach (GameObject t in l_EgyptTorchs)
            {
                t.GetComponentInChildren<ParticleSystem>().Play(true);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    //EGYPT SPECIAL EFFECTS
}
