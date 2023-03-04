using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWall : MonoBehaviour
{
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private int amount;

    private int offsetZ = 6;

    private void Start() {
        for (int i = 0; i < amount; i++)
        {
            int z = i * offsetZ;
            Instantiate(wallPrefab,new Vector3(0,0,z),Quaternion.identity,transform);
        }
    } 
}
