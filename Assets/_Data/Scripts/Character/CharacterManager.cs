using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<Character> listCharacter = new List<Character>();

    private void Awake() {
        Character.OnAnyCharacterSpawned += Character_OnAnyCharacterSpawned;
        Character.OnAnyCharacterDeath += Character_OnAnyCharacterDeath;
    }

    private void Character_OnAnyCharacterSpawned(Character character)
    {
        listCharacter.Add(character);
    }

    private void Character_OnAnyCharacterDeath(Character character)
    {
        listCharacter.Add(character);
    }

    public List<Character> GetListCharacter()
    {
        return listCharacter;
    }
}
