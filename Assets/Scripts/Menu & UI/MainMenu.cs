using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        VariableInitialization();
    }
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

    void VariableInitialization()
    {
        if(!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin",0);
        }
        if(!PlayerPrefs.HasKey("WeaponCooldownLevel"))
        {
            PlayerPrefs.SetFloat("WeaponCooldownLevel",0);
        }
        if(!PlayerPrefs.HasKey("PlayerSpeedLevel"))
        {
            PlayerPrefs.SetFloat("PlayerSpeedLevel",0);
        }
        if(!PlayerPrefs.HasKey("WeaponDamageLevel"))
        {
            PlayerPrefs.SetFloat("WeaponDamageLevel",0);
        }
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore",0);
        }
        if(!PlayerPrefs.HasKey("PlayerTorchLevel"))
        {
            PlayerPrefs.SetInt("PlayerTorchLevel",0);
        }
    }
}
