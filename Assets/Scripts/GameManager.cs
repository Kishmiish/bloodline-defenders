using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EnemyGenerator enemyGenerator;
    public bool isPlayerAlive;

    void Start()
    {
        isPlayerAlive = true;
    }


    void Update()
    {
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
        enemyGenerator.CancelInvoke();
        //enemyGenerator.PlayerDied();
        //Time.timeScale = 0f;
    }
}
