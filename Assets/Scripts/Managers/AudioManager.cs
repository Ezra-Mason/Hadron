using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //class for the sound data that we want to use
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f,1f)]
        public float volume;
        [Range(0.1f, 3f)]
        public float pitch;
        [HideInInspector]
        public AudioSource source;
        public bool looping;
    }

    public Sound[] sounds;
    public static AudioManager instance = null;

    public void Awake()
    {
        //singleton pattern
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //add sounds to manager
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
        }

        //play the main music when manager starts
        Play("Soundtrack");
    }

    //play sound method
    public void Play(string name)
    {
        //get the sound if its valid
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
        {
            Debug.Log("Sound " + name + " not found");
            return;
        }

        //apply small variations to short sound effects
        if (!s.source.loop)
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-0.1f, 0.1f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-0.02f, 0.02f));
        }

        //pitch sound down when the game is paused
        if (GameMenuManager.gameIsPaused)
        {
            s.source.pitch *= 0.5f;
        }

        s.source.Play(); 
    }

    //method for stopping a sound that is playing
    public void Stop(string name)
    {
        //find the input sound and check its valid
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found");
            return;
        }

        s.source.Stop();
    }
}
