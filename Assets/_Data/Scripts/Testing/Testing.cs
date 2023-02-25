using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform unitPrefabs;
    [SerializeField] private int number = 10;

    private void Start() {
        StartCoroutine(SpawnUnit());
    }

    IEnumerator SpawnUnit()
    {
        for (int i = 0; i < number; i++)
        {
            Transform unit = Instantiate(unitPrefabs,transform.position,Quaternion.identity,transform);
            yield return new WaitForSeconds(0.5f);
            BaseAction action = unit.GetComponent<Unit>().GetAction<MoveAction>();
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition( new Vector3(20,0,20));
            action.TakeAction(gridPosition,Clear);
        }
    }

    void Clear()
    {

    }

    
}
