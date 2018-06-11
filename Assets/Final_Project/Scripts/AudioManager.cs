using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public Manager manager;
    public static AudioManager instance;
    private float generalVolumeFactor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        generalVolumeFactor = 1f;
        //Play("Theme");
    }

    /*
        Hace sonar un temazo pero si se vuelve a hacer sonar para la anterior 
    */
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Not sound FOUND");
            return;
        }
        s.source.Play();
    }

    /*
        Hace sonar un temazo y lo superpone al anterior si vuelve a llamarse 
    */
    public AudioSource PlayShoot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Not sound FOUND");
            return null;
        }
        s.source.PlayOneShot(s.clip);
        return s.source;
    }

    /*
        TODO: ESTO NO FUNCIONA BIEN HAY QUE VER COMO SE MANEJAN LAS REFERENCIAS
        Para de sonar el temazo en cuestion
    */
    public AudioSource StopSound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Not sound FOUND");
            return null;
        }

        s.source.Stop();
        return s.source;
    }

    public void SetGeneralVolume(float volumeValue)
    {
        generalVolumeFactor = Map(Mathf.Clamp(volumeValue, 0f, 10f), 0f, 10f, 0f, 1f);
        SetMusicVolume(volumeValue);
        SetFxVolume(volumeValue);
    }

    public void SetMusicVolume(float volumeValue)
    {
        Mathf.Clamp(volumeValue, 0f, 10f);
        float vol = Map(volumeValue, 0f, 10f, 0f, 1f);
        var auxSounds = from s in sounds where s.name.Contains("Music") select s;

        foreach (var s in auxSounds)
            s.source.volume = generalVolumeFactor * vol;
    }

    public void SetFxVolume(float volumeValue)
    {
        Mathf.Clamp(volumeValue, 0f, 10f);
        float vol = Map(volumeValue, 0f, 10f, 0f, 1f);
        var auxSounds = from s in sounds where s.name.Contains("Fx") select s;

        foreach (var s in auxSounds)
            s.source.volume = generalVolumeFactor * vol;
    }

    public void SetPitch(AudioSource aS, float pitch)
    {
        aS.pitch = pitch;
    }

    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void MuteAll()
    {
        foreach (var t in manager.templeController.l_TempleTorchs)
            t.GetComponent<AudioSource>().mute = true;

        foreach (var f in manager.templeController.l_flamethrower)
            f.GetComponent<AudioSource>().mute = true;

        foreach (var t in manager.egyptController.l_EgyptTorchs)
            t.GetComponent<AudioSource>().mute = true;

        foreach (var w in manager.egyptController.l_waterfalls)
            w.GetComponent<AudioSource>().mute = true;

        foreach (var r in manager.villageController.l_VillageRain)
            r.GetComponent<AudioSource>().mute = true;

        manager.villageController.GetComponent<AudioSource>().mute = true;
    }

    public void UnMuteAll()
    {
        foreach (var t in manager.templeController.l_TempleTorchs)
            t.GetComponent<AudioSource>().mute = false;

        foreach (var f in manager.templeController.l_flamethrower)
            f.GetComponent<AudioSource>().mute = false;

        foreach (var t in manager.egyptController.l_EgyptTorchs)
            t.GetComponent<AudioSource>().mute = false;

        foreach (var w in manager.egyptController.l_waterfalls)
            w.GetComponent<AudioSource>().mute = false;

        foreach (var r in manager.villageController.l_VillageRain)
            r.GetComponent<AudioSource>().mute = false;

        manager.villageController.GetComponent<AudioSource>().mute = false;
    }
}
