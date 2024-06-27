using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterPreview : MonoBehaviour
{
    [SerializeField] private Sprite[] characters;
    [SerializeField] private GameObject[] prefabs;
    private static GameObject selectedPlayer;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetPreview(int characterNumber)
    {
        image.sprite = characters[characterNumber];
        PlayerPrefs.SetInt("Character",characterNumber);
    }

    public void HostGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public static GameObject GetSelectedPlayer()
    {
        return selectedPlayer;
    }
}
