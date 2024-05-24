using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        
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
}
