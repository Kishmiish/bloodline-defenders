using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private int chunkSize;
    [SerializeField] private float delayTime;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    void Start(){
        InvokeRepeating(nameof(ObjectDestruction), delayTime, delayTime);
    }

    void ObjectDestruction(){
        if(Math.Abs(transform.position.x - player.transform.position.x) > chunkSize / 2 || Math.Abs(transform.position.y - player.transform.position.y) > chunkSize / 2)
        {
            Destroy(gameObject);
        }
    }
}
