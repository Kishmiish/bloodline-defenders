using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private CharacterSelectDisplay characterSelect;
    private Character character;

    public void SetCharacter(CharacterSelectDisplay characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;
        this.character = character;
        this.characterSelect = characterSelect;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(character);
    }
}
