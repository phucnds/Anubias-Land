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
            MoveAction action = unit.GetComponent<Unit>().GetAction<MoveAction>();
            GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(unit.transform.position);
            GridPosition testGridPosition = LevelGrid.Instance.GetGridPosition( new Vector3(Random.Range(0,100),0,Random.Range(50,200)));


            yield return new WaitForSeconds(1.5f);
            if (!action.MoveTo(new Vector3(Random.Range(70, 100), 0, Random.Range(0, 200)),Clear)) {
                Debug.Log("Can't move");
                continue;
            }
        }
    }

    void Clear()
    {

    }

    
}
