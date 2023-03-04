using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : Building
{
    [SerializeField] private List<Character> characterInTown = new List<Character>();


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
    }

    public void CharacterLeaveTown(Character character)
    {
        characterInTown.Remove(character);
    }

}
