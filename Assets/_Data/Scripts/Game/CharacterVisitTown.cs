using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisitTown : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefabs;
    [SerializeField] private int amount = 5;


    private Building TownHall;

    private void Start()
    {
        StartCoroutine(Spawn());

        TownHall = GameMgr.Instance.BuildingManager.GetTownHall();
    }


    IEnumerator Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(2f);
            GameObject characterGameObject = Instantiate(characterPrefabs, transform.position, Quaternion.identity, transform);
            characterGameObject.name = "Character_" + i;
            Character character = characterGameObject.GetComponent<Character>();

            //VisitTown(character);
        }
    }


    private void VisitTown(Character character)
    {
        TownHall townHall = GameMgr.Instance.BuildingManager.GetTownHall();
        character.OrderInterupt(null, townHall.Interactable);
    }


}
