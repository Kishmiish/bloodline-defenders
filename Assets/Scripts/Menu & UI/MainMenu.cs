using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        ServerManager.Instance.StartHost();
    }

    public void JoinGame()
    {
        ServerManager.Instance.StartClient();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
