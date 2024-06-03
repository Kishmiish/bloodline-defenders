using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float minRadisu;
    [SerializeField] private float maxRadius;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    List<GameObject> enemies = new List<GameObject>();
    private float angle;
    private float distance;

    void Start()
    {
        InvokeRepeating(nameof(GenerateEnemy),5f,5f);
    }

    void FixedUpdate()
    {
        
    }

    void GenerateEnemy()
    {
        angle = UnityEngine.Random.Range(0, (float) Math.PI * 2f);
        distance = UnityEngine.Random.Range(minRadisu, maxRadius);
        GameObject enemyInstance = Instantiate(enemy, new Vector3((float)Math.Cos(angle) * distance, (float) Math.Sin(angle) * distance,0) + player.transform.position, quaternion.identity);
        Debug.Log(enemyInstance);
        enemies.Add(enemyInstance);
    }

    public void PlayerDied(){
        foreach (GameObject enemyInstance in enemies) {
            enemyInstance.GetComponent<Enemy>().Freeze();
        }
    }
}
