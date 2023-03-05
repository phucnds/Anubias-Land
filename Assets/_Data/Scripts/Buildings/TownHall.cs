using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TownHall : Building
{
    [SerializeField] private List<Character> characterInTown = new List<Character>();

    public static event UnityAction OnAnyCharacterStateChanged;

    private void TownHallUI_OnAnyCharacterStateChanged()
    {
        throw new NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();

        GameMgr.Instance.BuildingManager.SetTownHall(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void CharacterVisitTown(Character character)
    {
        characterInTown.Add(character);
        OnAnyCharacterStateChanged?.Invoke();
    }

    public void CharacterLeaveTown(Character character)
    {
        characterInTown.Remove(character);
        OnAnyCharacterStateChanged?.Invoke();
    }

    public List<Character> GetCharactersInTown()
    {
        return characterInTown;
    }

}
