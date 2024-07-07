using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCounter;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Slider abilityLevelSlider;
    [SerializeField] private TMP_Text abilityLevelText;
    [SerializeField] private String abilityName;
    [SerializeField] private int cost;

    void Awake()
    {
        CostCalculator();
        InitializeVariables();
    }
    void InitializeVariables()
    {
        abilityLevelSlider.value = PlayerPrefs.GetInt(abilityName);
        abilityLevelText.text = PlayerPrefs.GetInt(abilityName).ToString();
        SetCost();
    }
    public void Upgrade()
    {
        if(cost <= PlayerPrefs.GetInt("Coin"))
        {
            PlayerPrefs.SetInt(abilityName, PlayerPrefs.GetInt(abilityName) + 1);
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - cost);
            cost *= 2;
            InitializeVariables();
        }
    }
    void SetCost()
    {
        coinCounter.text = ": " + PlayerPrefs.GetInt("Coin").ToString();
        costText.text = cost.ToString();
    }
    void CostCalculator()
    {
        int level = PlayerPrefs.GetInt(abilityName);
        for (int i = 0; i < level; i++)
        {
            cost *= 2;
        }
    }
}
