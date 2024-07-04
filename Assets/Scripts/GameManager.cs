using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EnemyGenerator[] enemyGenerators;
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
