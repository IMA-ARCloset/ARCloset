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
    public int current_scene = 0, current_character = 0, total_characters = 5;
    public float sun_speed = 0.1f;
    public bool day = true, transition = false;


    private enum escenario { Temple, Village, Egypt }
    private Coroutine current_coroutine;

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
        current_coroutine = null;
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
        if (current_coroutine == null)
            current_coroutine = StartCoroutine(Transition());
        else
        {
            StopCoroutine(current_coroutine);
            current_coroutine = StartCoroutine(Transition());
        }
    }

    public void Temple_dawn()
    {
        if (current_coroutine == null)
            current_coroutine = StartCoroutine(Transition());
        else
        {
            StopCoroutine(current_coroutine);
            current_coroutine = StartCoroutine(Transition());
        }
    }

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
                // directional_light.transform.rotation = Quaternion.Slerp(directional_light.transform.rotation, night_transform.rotation, Time.time * sun_speed);
                // yield return null;
                yield return new WaitForSeconds(0.05f);

            }
            day = false;
        }
        else //  Amanece
        {
            for (int i = 0; i < 90; i++) //while (directional_light.transform.rotation != day_transform.rotation)
            {
                directional_light.transform.Rotate(new Vector3(0.5f, 0, 0), Space.World);
                // directional_light.transform.rotation = Quaternion.Slerp(directional_light.transform.rotation, day_transform.rotation, Time.time * sun_speed);
                // yield return null;
                yield return new WaitForSeconds(0.05f);
            }
            day = true;
        }
        transition = false;
    }

}
