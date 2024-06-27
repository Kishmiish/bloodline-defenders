using System;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class EnemyGenerator : NetworkBehaviour
{
    [SerializeField] private float minRadisu;
    [SerializeField] private float maxRadius;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnIntervalDecrease = 0.05f;
    private GameObject[] players;
    private float initialSpawnInterval;
    private float currentSpawnInterval;
    private int waveNumber;
    private int maxEnemiesPerWave;
    private float angle;
    private float distance;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        if(IsHost || IsServer)
        {
            maxEnemiesPerWave = 2;
            waveNumber = 1;
            initialSpawnInterval = 3f;
            currentSpawnInterval = initialSpawnInterval;
            InvokeRepeating(nameof(GenerateEnemyForPlayers),currentSpawnInterval,currentSpawnInterval);
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
        distance = UnityEngine.Random.Range(minRadisu, maxRadius);
        int numberOfEnemies = UnityEngine.Random.Range(1, maxEnemiesPerWave + 1);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float distanceBetweenEnemies = 1 + (i * 0.1f);
            var temp = Instantiate(enemy, new Vector3((float)Math.Cos(angle + distanceBetweenEnemies) * distance * distanceBetweenEnemies, (float) Math.Sin(angle + distanceBetweenEnemies) * distance * distanceBetweenEnemies, 0) + player.transform.position, quaternion.identity);
            temp.GetComponent<NetworkObject>().Spawn(true);
        }
        IncreaseDifficulty();
    }

    void IncreaseDifficulty()
    {
        waveNumber++;
        currentSpawnInterval = Mathf.Max(0.1f, currentSpawnInterval - spawnIntervalDecrease * waveNumber);
        maxEnemiesPerWave = Mathf.Min(10, maxEnemiesPerWave + 1);
    }
}
