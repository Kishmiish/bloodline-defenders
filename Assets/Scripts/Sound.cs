using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Added this so that the amount of Sound class instances can be controlled in inspector
[Serializable]
public class Sound
{
    public AudioClip audioClip;
    public String name;
    [Range(0f, 1f)]
    public float volume;
    public bool isLoop;

    [HideInInspector]
    public AudioSource audioSource;
}
