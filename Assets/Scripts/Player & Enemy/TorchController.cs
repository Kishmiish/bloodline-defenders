using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [SerializeField] private Light2D torch;
    void Start()
    {
        float radius = 4.5f;
        int level = PlayerPrefs.GetInt("PlayerTorchLevel");
        for (int i = 0; i < level; i++)
        {
            radius += 1f;
        }
        torch.pointLightOuterRadius = radius;
    }
}
