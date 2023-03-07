using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gatherable : MonoBehaviour
{

    public static event UnityAction<Gatherable> OnAnyGatherableCreated;
    public static event UnityAction<Gatherable> OnAnyGatherableDestroyed;

    public int value = 1000;    //Total quantity of this resource.

    [Header("Harvest")]
    public bool harvestable = true;     //If false, require a building to harvest (like building a Oil Pump on top of it)
    public float harvest_speed = 10f;   //Amount per game-hour harvested
    public bool tool_required = false;

    [Header("Anims")]
    public string harvest_anim;
    public AudioClip harvest_audio;

    //public int numberTreeReadyToChop = 6;
    //public bool isRegen = false;
    //private float timerRegen = 5;

    protected Interactable interact;

    private void Awake()
    {
        interact = GetComponent<Interactable>();
    }

    private void Start()
    {
        OnAnyGatherableCreated?.Invoke(this);
    }

    //private void Update()
    //{
    //    if (numberTreeReadyToChop < 6) isRegen = true;
    //    if (!isRegen) return;

    //    timerRegen -= Time.deltaTime;

    //    if (timerRegen <= 0)
    //    {
    //        numberTreeReadyToChop++;
    //        if (numberTreeReadyToChop == 6)
    //        {
    //            isRegen = false;
                
    //        }
    //        timerRegen = 5;
    //    }
    //}

    private void OnDestroy()
    {
        OnAnyGatherableDestroyed?.Invoke(this);
    }

    public Interactable Interactable { get { return interact; } }


    public static Gatherable GetNearest(Vector3 pos, float range = 999f, Gatherable ignore = null)
    {
        float min_dist = range;
        Gatherable nearest = null;
        foreach (Gatherable gather in GameMgr.Instance.GatherableManager.GetListGatherable())
        {
            float dist = (pos - gather.transform.position).magnitude;
            if (dist < min_dist) //&& gather.IsAlive() && gather != ignore
            {
                min_dist = dist;
                nearest = gather;
            }
        }
        return nearest;
    }

    public static Gatherable GetNearestUnassigned(Vector3 pos, float range = 999f)
    {
        float min_dist = range;
        Gatherable nearest = null;
        foreach (Gatherable gather in GameMgr.Instance.GatherableManager.GetListGatherable())
        {
            float dist = (pos - gather.transform.position).magnitude;
            if (dist < min_dist && !gather.Interactable.IsInteractFull())
            {
                min_dist = dist;
                nearest = gather;
            }
        }
        return nearest;
    }
}
