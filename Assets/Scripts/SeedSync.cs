using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SeedSync : NetworkBehaviour
{
    static float randomSeedFloat;
    public NetworkVariable<int> randomSeed;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsHost)
        {
            randomSeedFloat = Random.Range(0, 10000);
            randomSeed.Value = Mathf.FloorToInt(randomSeedFloat);
        }
    }
    public int GetRandomSeed()
    {
        return randomSeed.Value;
    }
}
