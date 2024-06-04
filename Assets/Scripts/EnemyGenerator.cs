using System;
using Unity.Mathematics;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float minRadisu;
    [SerializeField] private float maxRadius;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnIntervalDecrease = 0.05f;
    private float initialSpawnInterval;
    private float currentSpawnInterval;
    private int waveNumber;
    private int maxEnemiesPerWave;
    private float angle;
    private float distance;

    void Start()
    {
        maxEnemiesPerWave = 2;
        waveNumber = 1;
        initialSpawnInterval = 3f;
        currentSpawnInterval = initialSpawnInterval;
        InvokeRepeating(nameof(GenerateEnemy),currentSpawnInterval,currentSpawnInterval);
    }

    void GenerateEnemy()
    {
        angle = UnityEngine.Random.Range(0, (float) Math.PI * 2f);
        distance = UnityEngine.Random.Range(minRadisu, maxRadius);
        int numberOfEnemies = UnityEngine.Random.Range(1, maxEnemiesPerWave + 1);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemy, new Vector3((float)Math.Cos(angle) * distance, (float) Math.Sin(angle) * distance,0) + player.transform.position, quaternion.identity);
        }
        IncreaseDifficulty();
    }

    void IncreaseDifficulty()
    {
        waveNumber++;
        currentSpawnInterval = Mathf.Max(0.1f, currentSpawnInterval - spawnIntervalDecrease * waveNumber);
        maxEnemiesPerWave = Mathf.Max(10, maxEnemiesPerWave + 1);
    }
}
