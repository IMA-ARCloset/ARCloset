using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public TempleController templeController;
    public EgyptController egyptController;
    public VillageController villageController;

    public GameObject[] l_scenes;
    public GameObject[] l_characters;

    public Light directional_light;
    public Transform day_transform, night_transform;
    public int current_scene, current_character, total_characters, total_scenes;
    public float sun_speed = 0.1f;
    public bool day = true, transition = false, sound = false;
    public int changing = 0; //0: escenarios, 1: modelos
    public GameObject templeScene, villageScene, EgyptScene;

    private enum escenario { Temple, Village, Egypt }
    public Coroutine special_effectCorroutine;
    private bool special_effect;


    // Use this for initialization
    private void Start()
    {
        l_scenes = new GameObject[] { templeScene, villageScene, EgyptScene };
        total_scenes = l_scenes.Length;
        current_scene = 2;

        l_characters = GameObject.FindGameObjectsWithTag("Model");
        l_characters = l_characters.OrderBy(go => go.name).ToArray();
        total_characters = l_characters.Length;
        current_character = 0;

        special_effect = false;

        for (int i = 0; i < total_scenes; i++)
            if (i != current_scene)
                l_scenes[i].SetActive(false);

        for (int i = 0; i < total_characters; i++)
            if (i != current_character)
                l_characters[i].SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("n") && !transition)
            Change_dayNight();

        if (Input.GetKey("e") && special_effectCorroutine == null)
        {
            Special_effect();
            Debug.Log("LLAMAMAMOM");
        }
    }

    public void Change_scene(int n)
    {
        if (n >= 0 && n < total_scenes)
        {
            l_scenes[current_scene].SetActive(false); // Desactivamos la escena en la que nos encontramos
            l_scenes[n].SetActive(true); // Activamos la escena siguiente
            current_scene = n;
        }
        else if (n == total_scenes)
        {
            l_scenes[current_scene].SetActive(false);
            l_scenes[0].SetActive(true);
            current_scene = 0;
        }
        else if (n < 0)
        {
            l_scenes[current_scene].SetActive(false);
            l_scenes[total_scenes - 1].SetActive(true);
            current_scene = total_scenes - 1;
        }
    }

    public void Change_character(int n)
    {
        if (n >= 0 && n < total_characters)
        {
            l_characters[current_character].SetActive(false); // Desactivamos el modelo en el que nos encontramos
            l_characters[n].SetActive(true); // Activamos el modelo siguiente
            current_character = n;
        }
        else if (n == total_characters)
        {
            l_characters[current_character].SetActive(false);
            l_characters[0].SetActive(true);
            current_character = 0;
        }
        else if (n < 0)
        {
            l_characters[current_character].SetActive(false);
            l_characters[total_characters - 1].SetActive(true);
            current_character = total_characters - 1;
        }
    }

    public void Special_effect()
    {
        switch (current_scene)
        {
            case (int)escenario.Temple:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(templeController.Temple_specialEffect());
                break;

            case (int)escenario.Village:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(villageController.Village_specialEffect());
                break;

            case (int)escenario.Egypt:
                if (special_effectCorroutine == null)
                    special_effectCorroutine = StartCoroutine(egyptController.Egypt_specialEffect());
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
    //templo
    public void Temple_nightfall()
    {
        StartCoroutine(Transition());
    }

    public void Temple_dawn()
    {
        StartCoroutine(Transition());
    }

    //Egipto
    public void Egypt_nightfall()
    {
        StartCoroutine(Transition());
    }

    public void Egypt_dawn()
    {
        StartCoroutine(Transition());
    }

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
            for (int i = 0; i < 120; i++)
            {
                directional_light.transform.Rotate(new Vector3(-0.5f, 0, 0), Space.World);
                yield return new WaitForSeconds(0.05f);
            }
            day = false;
            Manage_illumination();
        }
        else //  Amanece
        {
            for (int i = 0; i < 120; i++)
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
                StartCoroutine(templeController.Manage_templeTorchs());
                break;

            case (int)escenario.Egypt:
                StartCoroutine(egyptController.Manage_EgyptTorchs());
                break;

            case (int)escenario.Village:
                StartCoroutine(villageController.Manage_VillageLights());
                break;
        }
    }
}