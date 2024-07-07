using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private GameObject visuals;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text cooldown;
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text weapon;
    [SerializeField] private Image weaponIcon;

    public void UpdateDisplay(Character hero)
    {
        if (hero.Id != -1)
        {
            var character = characterDatabase.GetCharacterById(hero.Id);
            weaponIcon.sprite = character.WeaponIcon;
            PlayerController playerController = character.GameplayPrefab.GetComponent<PlayerController>();
            health.text = playerController.GetMaxHealth().ToString();
            WeaponController weaponController = character.GameplayPrefab.GetComponentInChildren<WeaponController>();
            cooldown.text = weaponController.GetInitialTimeToAttack().ToString();
            damage.text = weaponController.GetInitialWeaponDamage().ToString();
        }
    }
    public void DisableDisplay()
    {
    }
}
