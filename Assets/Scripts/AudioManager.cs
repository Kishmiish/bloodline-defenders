using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioMixer audioMixer;
    private GameObject audioMixerHolder;
    private bool isMuted = false;
    private float currentVolume;
    private static AudioManager instance;
    void Awake()
    {
        audioMixerHolder = GameObject.Find("Audiomixergroup");
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.loop = s.isLoop;
            s.audioSource.outputAudioMixerGroup = audioMixerHolder.GetComponent<audiomixertest>().audioMixerGroup;
        }
    }

    void Start()
    {
        play("Theme");
    }

    public void play(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
        currentVolume = volume;
    }

    public void volumeOnOff()
    {
        isMuted = !isMuted;
        if(isMuted){
            audioMixer.SetFloat("volume",-80f);
        }
        else{
            setVolume(currentVolume);
        }
        
    }

}
