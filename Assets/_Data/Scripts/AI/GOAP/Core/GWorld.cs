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
    private static ResourceQueue patients;
    private static ResourceQueue cubicles;
    private static ResourceQueue offices;
    private static ResourceQueue toilets;
    private static ResourceQueue puddles;
    

    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld()
    {
        world = new WorldStates();
        patients = new ResourceQueue("", "", world);
        cubicles = new ResourceQueue("Cubicle", "FreeCubicle", world);
        offices = new ResourceQueue("Office", "FreeOffice", world);
        toilets = new ResourceQueue("Toilet", "FreeToilet", world);
        puddles = new ResourceQueue("Puddle", "FreePuddle", world);

        resources.Add("patients", patients);
        resources.Add("cubicles", cubicles);
        resources.Add("offices", offices);
        resources.Add("toilets", toilets);
        resources.Add("puddles", puddles);
       
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
