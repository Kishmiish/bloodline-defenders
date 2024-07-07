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
    [SerializeField] private EnemyGenerator[] enemyGenerators;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TMP_Text yourScore;
    [SerializeField] private TMP_Text highScore;
    public bool isPlayerAlive;
    private int currentLevel;
    private int prevLevel;
    private float elpasedTime;
    void Start()
    {
        currentLevel = 0;
        prevLevel = currentLevel;
        isPlayerAlive = true;
    }


    void Update()
    {
        elpasedTime += Time.deltaTime;
        currentLevel = Mathf.FloorToInt(elpasedTime % 60) / 5;
        if(currentLevel != prevLevel){
            IncreaseDifficulty();
            prevLevel = currentLevel;
        }
        if(Input.GetKey(KeyCode.Escape)){
            pause();
        }
    }

    void pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void unPause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void GameOver(){
        isPlayerAlive = false;
        foreach(EnemyGenerator enemyGenerator in enemyGenerators)
        {
            enemyGenerator.CancelInvoke();

        }
        yourScore.text = GetComponent<KillCounter>().GetKillCount().ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        gameOverMenu.SetActive(true);
    }

    public void Quit()
    {
        if(GetComponent<KillCounter>().GetKillCount() > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", GetComponent<KillCounter>().GetKillCount());
        }
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        if(NetworkManager.Singleton.IsHost){
            NetworkManager.Singleton.Shutdown();
        }
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
    
}
