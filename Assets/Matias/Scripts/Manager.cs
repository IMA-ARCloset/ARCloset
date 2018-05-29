using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject[] l_scenes;
    public GameObject[] l_characters;
    public Light directional_light;
    public Transform day_transform, night_transform;
    public int current_scene = 0, current_character, total_characters;
    public float sun_speed = 0.1f;
    public bool day = true, transition = false, sound = false;
    public int changing = 0; //0: escenarios, 1: modelos

    private enum escenario { Temple, Village, Egypt }


    /*
		Habra alguans escenas que sean diferentes de noche y de dia por eso de los lightMaps
		y en otras simplemente cambiaremos la orientacion del skybox y pondremos cuatro lamparas
	*/
    private int total_scenes = Enum.GetNames(typeof(escenario)).Length + 1;


    // Use this for initialization
    private void Start()
    {
        l_scenes = new GameObject[total_scenes]; // +1 xq la Village son dos escenarios con light maps diferentes
        l_characters = GameObject.FindGameObjectsWithTag("Model");

        total_characters = l_characters.Length;
        current_character = 0;

        for(int i=1; i<total_characters; i++)
            l_characters[i].SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKey("n") && !transition)
            Change_dayNight();
    }

    public void Change_scene(int n)
    {
        if (n > 0 && n < total_scenes)
        {
            l_scenes[current_scene].SetActive(false); // Desactivamos la escena en la que nos encontramos
            l_scenes[n].SetActive(true); // Activamos la escena siguiente
            current_scene = n;
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
            l_characters[total_characters-1].SetActive(true);
            current_character = total_characters - 1;
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

                    break;

                case (int)escenario.Egypt:

                    break;

                default:
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

                break;

            case (int)escenario.Egypt:

                break;

            default:
                break;
        }

    }

    /*
		FUNCIONES PARA HACER DE NOCHE EL MUNDO 
	*/

    public void Temple_nightfall()
    {
        //directional_light.transform.rotation = Quaternion.Slerp(day_transform.rotation, night_transform.rotation, Time.time * sun_speed);
        StartCoroutine(Transition());
    }

    public void Temple_dawn()
    {
        StartCoroutine(Transition());
    }

    /*
        Gestiona el ciclo dia noche
    */
    IEnumerator Transition()
    {
        if (day) // Anochece
        {
            while (directional_light.transform.rotation != night_transform.rotation)
            {
                directional_light.transform.rotation = Quaternion.Slerp(day_transform.rotation, night_transform.rotation, Time.time * sun_speed);
                yield return null;

            }
            day = false;
        }
        else //  Amanece
        {
            while (directional_light.transform.rotation != day_transform.rotation)
            {
                directional_light.transform.rotation = Quaternion.Slerp(day_transform.rotation, night_transform.rotation, Time.time * sun_speed);
                yield return null;
            }
            day = true;
        }
        transition = false;
    }

}
