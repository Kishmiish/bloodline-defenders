using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private GameObject networkManager;
    [SerializeField] private SeedSync seedSync;
    public static ServerManager Instance{get; private set;}
    public Dictionary<ulong, ClientData> ClientData {get; private set;}
    private bool gameHasStarted;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        ClientData = new Dictionary<ulong, ClientData>();
        NetworkManager.Singleton.StartServer();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        ClientData = new Dictionary<ulong, ClientData>();
        NetworkManager.Singleton.StartHost();
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(gameHasStarted)
        {
            response.Approved = false;
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);
    }

    private void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClinetDisconnect;
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    private void OnClinetDisconnect(ulong clientId)
    {
        if(ClientData.ContainsKey(clientId))
        {
            ClientData.Remove(clientId);
        }
    }

    public void SetCharacter(ulong clientId, int characterId)
    {
        if(ClientData.TryGetValue(clientId, out ClientData data))
        {
            data.characterId = characterId;
        }
    }

    public void StartGame()
    {
        gameHasStarted = true;
        NetworkManager.Singleton.SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StopHost()
    {
        if(NetworkManager.Singleton != null)
        {
            if(NetworkManager.Singleton.IsHost)
            {
                seedSync.StopClientRPC();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
                NetworkManager.Singleton.OnServerStarted -= OnNetworkReady;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClinetDisconnect;
                StartCoroutine(DelayedHostShutdown());
            }
        }
    }
    private IEnumerator DelayedHostShutdown()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(networkManager);
        NetworkManager.Singleton.Shutdown();
    }
    public void StopClient()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted -= OnNetworkReady;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClinetDisconnect;
            NetworkManager.Singleton.Shutdown();
            Destroy(gameObject);
            Destroy(networkManager);
        }
    }
}
