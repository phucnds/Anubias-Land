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
            Character character = characterGameObject.GetComponent<Character>();

            //VisitTown(character);
        }
    }


    private void VisitTown(Character character)
    {
        ActionWalkingAround walking = ActionBasic.Get<ActionWalkingAround>();
        character.OrderInterupt(walking, null);
    }


}
