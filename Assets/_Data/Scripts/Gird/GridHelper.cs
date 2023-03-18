using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper 
{
    public static Vector3 ConvertPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x / 2) + 99;
        int z = Mathf.RoundToInt(pos.z / 2) + 99;
        return (Vector3)GameMgr.Instance.GetGridGraph().GraphPointToWorld(x, z, 0);
    }
}
