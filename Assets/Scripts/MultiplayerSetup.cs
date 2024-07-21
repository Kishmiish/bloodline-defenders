using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class MultiplayerSetup : NetworkBehaviour
{
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private TMP_InputField port;
    public void SetupGame()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ip.text;
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(port.text);
    }
    public void StartOffline()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "127.0.0.1";
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = 7777;
    }
}
