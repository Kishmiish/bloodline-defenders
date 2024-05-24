using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerFinder : MonoBehaviour
{
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void play(String name)
    {
        audioManager.play(name);
    }

    public void setVolume(float volume)
    {
        audioManager.setVolume(volume);
    }

    public void volumeOnOff()
    {
        audioManager.volumeOnOff();
    }
}
