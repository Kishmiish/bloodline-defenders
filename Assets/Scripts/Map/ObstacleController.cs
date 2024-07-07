using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private GameObject[] players;
    private GameObject[] deadPlayers;
    static private int chunkSize = 60;
    private float delayTime = 3f;
    void Start(){
        InvokeRepeating(nameof(ObjectDestruction), delayTime, delayTime);
    }

    void ObjectDestruction(){
        players = GameObject.FindGameObjectsWithTag("Player");
        bool remove = true;
        foreach (GameObject player in players)
        {
            if(Math.Abs(transform.position.x - player.transform.position.x) < chunkSize / 2 && Math.Abs(transform.position.y - player.transform.position.y) < chunkSize / 2)
            {
                remove = false;
                return;
            }
        }
        deadPlayers = GameObject.FindGameObjectsWithTag("Dead");
        foreach (var player in deadPlayers)
        {
            if(Math.Abs(transform.position.x - player.transform.position.x) < chunkSize / 2 && Math.Abs(transform.position.y - player.transform.position.y) < chunkSize / 2)
            {
                remove = false;
                return;
            }
        }
        if(remove)
        {
            Destroy(gameObject);
        }
    }
}
