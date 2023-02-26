using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceQueue
{
    public Queue<GameObject> que = new Queue<GameObject>();
    public string tag;
    public string modState;

    public ResourceQueue(string t, string ms, WorldStates w)
    {
        tag = t;
        modState = ms;

        if (tag != "")
        {
            GameObject[] resource = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject r in resource)
                que.Enqueue(r);
        }

        if (modState != "")
        {
            w.ModifyState(modState, que.Count);
        }
    }

    public void AddResource(GameObject r)
    {
        que.Enqueue(r);
    }

    public GameObject RemoveResource()
    {
        if (que.Count == 0) return null;
        return que.Dequeue();
    }
}

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static ResourceQueue clients;
    private static ResourceQueue tables;
    private static ResourceQueue orderTables;
    

    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld()
    {
        world = new WorldStates();
        clients = new ResourceQueue("", "", world);
        tables = new ResourceQueue("Table", "FreeTable", world);
        orderTables = new ResourceQueue("", "", world);

        resources.Add("clients", clients);
        resources.Add("tables", tables);
        resources.Add("orderTables",orderTables);
       
    }

    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }

    private GWorld() { }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
