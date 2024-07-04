using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayfabManager : MonoBehaviour
{

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField password;

    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest {
            Email = emailInput.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest {
            Email = emailInput.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        SceneManager.LoadScene("Menu");
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.GenerateErrorReport();
    }
}