using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGAgent : MonoBehaviour
{
    [SerializeField] private Transform agent;
    [SerializeField] private int numbers;


    private void Start() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < numbers; i++)
        {
            yield return new WaitForSeconds(10);
            Instantiate(agent,transform.position,Quaternion.identity,transform);
        }
    }
}
