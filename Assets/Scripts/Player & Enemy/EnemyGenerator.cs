using System;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class EnemyGenerator : NetworkBehaviour
{
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private Enemy enemy;
    static private float spawnInterval = 1.2f;
    private GameObject[] players;
    private float angle;
    private float distance;

    void Awake()
    {
        spawnInterval = 1f;
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        if(IsHost || IsServer)
        {
            InvokeRepeating(nameof(GenerateEnemyForPlayers),0,spawnInterval);
        }
    }

    void GenerateEnemyForPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject temp in players)
        {
            GenerateEnemy(temp);
        }
    }

    void GenerateEnemy(GameObject player)
    {
        angle = UnityEngine.Random.Range(0, (float) Math.PI * 2f);
        distance = UnityEngine.Random.Range(minRadius, maxRadius);
        var temp = Instantiate(enemy, new Vector3((float)Math.Cos(angle) * distance, (float) Math.Sin(angle) * distance, 0) + player.transform.position, quaternion.identity);
        temp.GetComponent<NetworkObject>().Spawn(true);
    }

    public void IncreaseDifficulty()
    {
        spawnInterval *= 0.95f;
        enemy.IncreaseDifficulty();
    }
}
