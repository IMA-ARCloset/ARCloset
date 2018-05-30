using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject[] l_scenes;
    public GameObject[] l_characters;
    public GameObject[] l_TempleTorchs;
    public GameObject[] l_EgyptTorchs;
    public GameObject[] l_flamethrower;
    public GameObject[] l_waterfalls;
    public GameObject[] l_VillageLights;
    public GameObject[] l_VillageFire;

    public Light directional_light;
    public Transform day_transform, night_transform;
    public int current_scene = 2, current_character = 0, total_characters = 5;
    public float sun_speed = 0.1f;
    public bool day = true, transition = false;


    private enum escenario { Temple, Village, Egypt }
    private Coroutine current_coroutine, special_effectCorroutine;
    private bool special_effect;
    /*
		Habra alguans escenas que sean diferentes de noche y de dia por eso de los lightMaps
		y en otras simplemente cambiaremos la orientacion del skybox y pondremos cuatro lamparas
	*/
    private int total_scenes = Enum.GetNames(typeof(escenario)).Length + 1;


    // Use this for initialization
    private void Start()
    {
        l_scenes = new GameObject[total_scenes]; // +1 xq la Village son dos escenarios con light maps diferentes
        l_characters = new GameObject[total_characters];

        l_TempleTorchs = GameObject.FindGameObjectsWithTag("Torch");
        l_TempleTorchs = l_TempleTorchs.OrderBy(go => go.name).ToArray();

        l_flamethrower = GameObject.FindGameObjectsWithTag("FlameThrower");
        l_flamethrower = l_flamethrower.OrderBy(go => go.name).ToArray();

        l_EgyptTorchs = GameObject.FindGameObjectsWithTag("EgyptTorch");

        l_waterfalls = GameObject.FindGameObjectsWithTag("Waterfalls");

        l_VillageLights = GameObject.FindGameObjectsWithTag("VillageLight");

        l_VillageFire = GameObject.FindGameObjectsWithTag("VillageFire");

        special_effect = false;

        foreach (GameObject gO in l_TempleTorchs)
        {
            gO.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        foreach (GameObject gO in l_EgyptTorchs)
        {
            gO.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        foreach (GameObject gO in l_flamethrower)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        foreach (GameObject gO in l_waterfalls)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        /*
            Efectos del pueblo
        */
        foreach (GameObject gO in l_VillageLights)
        {
            gO.SetActive(false);
        }

        foreach (GameObject gO in l_VillageFire)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }


    private void Update()
    {
        if (Input.GetKey("n") && !transition)
            Change_dayNight();

        if (Input.GetKey("e") && special_effectCorroutine == null)
        {
            Special_effect();
            Debug.Log("LLAMAMAMOM");
        }


        // if (special_effectCorroutine == null)
        //     Debug.Log("NO HAY NAH");
    }

    public void Change_scene(int n)
    {
        if (n > 0 && n < total_scenes)
        {
            l_scenes[current_scene].SetActive(false); // Desactivamos la escena en la que nos encontramos
            l_scenes[n].SetActive(true); // Activamos la escena siguiente
        }
    }

    public void Change_character(int n)
    {
        if (n > 0 && n < total_characters)
        {
            l_characters[current_character].SetActive(false); // Desactivamos la escena en la que nos encontramos
            l_characters[n].SetActive(true); // Activamos la escena siguiente
        }
    }

    public void Special_effect()
    {
        switch (current_scene)
        {
            case (int)escenario.Temple:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(Temple_specialEffect());
                break;

            case (int)escenario.Village:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(Village_specialEffect());
                break;

            case (int)escenario.Egypt:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(Egypt_specialEffect());
                break;
        }
    }


    public void Change_dayNight()
    {
        transition = true;
        if (day)
        {
            switch (current_scene)
            {
                case (int)escenario.Temple:

                    Temple_nightfall();
                    break;

                case (int)escenario.Village:
                    Village_nightfall();
                    break;

                case (int)escenario.Egypt:
                    Egypt_nightfall();
                    break;
            }

            return;
        }

        switch (current_scene)
        {
            case (int)escenario.Temple:

                Temple_dawn();
                break;

            case (int)escenario.Village:
                Village_dawn();
                break;

            case (int)escenario.Egypt:
                Egypt_dawn();
                break;
        }

    }

    /*
		FUNCIONES PARA GESTIONAR EL CICLO DIA NOCHE
	*/


    #region 
    // Templo
    public void Temple_nightfall()
    {
        //directional_light.transform.rotation = Quaternion.Slerp(day_transform.rotation, night_transform.rotation, Time.time * sun_speed);
        StartCoroutine(Transition());
    }

    public void Temple_dawn()
    {
        StartCoroutine(Transition());
    }

    // Egipto
    public void Egypt_nightfall()
    {
        StartCoroutine(Transition());
    }

    public void Egypt_dawn()
    {
        StartCoroutine(Transition());
    }

    // Pueblo
    public void Village_nightfall()
    {
        StartCoroutine(Transition());
    }

    public void Village_dawn()
    {
        StartCoroutine(Transition());
    }
    #endregion
    /*
        Gestiona el ciclo dia noche
    */
    IEnumerator Transition()
    {
        if (day) // Anochece
        {
            for (int i = 0; i < 90; i++) //while (directional_light.transform.rotation != night_transform.rotation)
            {
                directional_light.transform.Rotate(new Vector3(-0.5f, 0, 0), Space.World);
                yield return new WaitForSeconds(0.05f);
            }
            day = false;
            Manage_illumination();
        }
        else //  Amanece
        {
            for (int i = 0; i < 90; i++) //while (directional_light.transform.rotation != day_transform.rotation)
            {
                directional_light.transform.Rotate(new Vector3(0.5f, 0, 0), Space.World);
                yield return new WaitForSeconds(0.05f);
            }
            day = true;
            Manage_illumination();
        }
        transition = false;
    }

    void Manage_illumination()
    {
        switch (current_scene)
        {
            case (int)escenario.Temple:
                StartCoroutine(Manage_templeTorchs());
                break;

            case (int)escenario.Egypt:
                StartCoroutine(Manage_EgyptTorchs());
                break;

            case (int)escenario.Village:
                Manage_VillageLights();
                break;
        }
    }

    /*
        Gestiona las antorchas
    */
    IEnumerator Manage_templeTorchs()
    {

        if (day)
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

    IEnumerator Manage_EgyptTorchs()
    {

        if (day)
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

    IEnumerator Manage_VillageLights()
    {
        if (day)
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
            foreach (GameObject t in l_EgyptTorchs)
            {
                //t.GetComponentInChildren<ParticleSystem>().Play(true);
                t.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    /* 
        De aqui para abajo todos los efectos especiales
    */

    IEnumerator Temple_specialEffect()
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

        special_effectCorroutine = null;
    }


    IEnumerator Egypt_specialEffect()
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

        special_effectCorroutine = null;
    }

    private IEnumerator Village_specialEffect()
    {
        foreach (GameObject gO in l_VillageFire)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Play(true);
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject gO in l_VillageFire)
        {
            var l_aux = gO.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in l_aux)
                p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        special_effectCorroutine = null;
    }


}