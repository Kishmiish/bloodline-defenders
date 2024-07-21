using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private EnemyGenerator[] enemyGenerators;
    [SerializeField] private TMP_Text yourScore;
    [SerializeField] private TMP_Text highScore;
    public bool isPlayerAlive;
    private int currentLevel;
    private int prevLevel;
    private float elpasedTime;
    void Start()
    {
        upgradeMenu.SetActive(false);
        currentLevel = 0;
        prevLevel = currentLevel;
        isPlayerAlive = true;
    }


    void Update()
    {
        if(upgradeMenu == null) { GameObject.FindGameObjectWithTag("UpgradeMenu"); }
        elpasedTime += Time.deltaTime;
        currentLevel = Mathf.FloorToInt(elpasedTime % 60) / 5;
        if(currentLevel != prevLevel){
            IncreaseDifficulty();
            prevLevel = currentLevel;
        }
        if(Input.GetKey(KeyCode.Escape)){
            pause();
        } else if (Input.GetKey(KeyCode.Tab))
        {
            upgradeMenu.SetActive(true);
        }
    }

    void pause()
    {
        pauseMenu.SetActive(true);
        if(ServerManager.Instance.ClientData.Count == 1)
        {
            Time.timeScale = 0f;
        }
    }

    public void unPause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void GameOver(){
        isPlayerAlive = false;
        gameOverMenu.SetActive(true);
        foreach(EnemyGenerator enemyGenerator in enemyGenerators)
        {
            enemyGenerator.CancelInvoke();
        }
        yourScore.text = GetComponent<KillCounter>().GetKillCount().ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void Quit()
    {
        if(GetComponent<KillCounter>().GetKillCount() > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", GetComponent<KillCounter>().GetKillCount());
        }
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        if(NetworkManager.Singleton.IsHost)
        {
            ServerManager.Instance.StopHost();
        } else if (NetworkManager.Singleton.IsClient)
        {
            ServerManager.Instance.StopClient();   
        }
        Time.timeScale = 1f;
        Destroy(GameObject.FindGameObjectWithTag("SeedSyncer"));
        SceneManager.LoadScene("Menu");
    }

    void IncreaseDifficulty()
    {
        for (int i = 1; i < currentLevel / 5; i++)
        {
            enemyGenerators[i].enabled = true;
        }
        foreach (EnemyGenerator enemyGenerator in enemyGenerators)
        {
            if(enemyGenerator.isActiveAndEnabled){
                enemyGenerator.IncreaseDifficulty();
            }
        }
    }
    public void UpgradeMenu()
    {
        upgradeMenu.SetActive(true);
        try
        {
            if(ServerManager.Instance.ClientData.Count == 1) { Time.timeScale = 0f; }
        }
        catch (System.Exception) {}
    }
}
