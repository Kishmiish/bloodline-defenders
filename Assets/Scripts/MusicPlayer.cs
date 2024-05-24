using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.play("Chronicles");
    }
}
