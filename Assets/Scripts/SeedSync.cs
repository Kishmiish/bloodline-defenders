using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeedSync : NetworkBehaviour
{
    public static SeedSync Instance{get; private set;}
    static float randomSeedFloat;
    public NetworkVariable<int> randomSeed;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    [ClientRpc]
    public void StopClientRPC()
    {
        ServerManager.Instance.StopClient();
        SceneManager.LoadScene("Menu");
        Destroy(gameObject);
        NetworkManager.Singleton.Shutdown();
    }
}
