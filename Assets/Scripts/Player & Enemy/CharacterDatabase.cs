using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Characters/Database")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private Character[] characters = new Character[0];

    public Character[] GetAllCharacters() => characters;

    public Character GetCharacterById(int characterId)
    {
        foreach (Character character in characters)
        {
            if(character.Id == characterId)
            {
                return character;
            }
        }

        return null;
    }
}
