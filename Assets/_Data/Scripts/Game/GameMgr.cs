using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private GatherableManager gatherableManager;
    [SerializeField] private StorageManager storageManager;
    [SerializeField] private CivilianManager civilianManager;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private DestructibleManager destructManager;

   

    public static GameMgr Instance { get; private set; }

    [SerializeField] private float timeScale = 1;
    private float speed_multiplier = 1f;


    [SerializeField] private AstarPath path;

    private AstarData data;
    private GridGraph gridGraph;

    private void Awake()
    {
        Instance = this;
        LoadData();
        Time.timeScale = timeScale;

        data = path.data;
        gridGraph = data.gridGraph;

    }

    private void LoadData()
    {
        ActionBasic.Load();
        WorkBasic.Load();
        AttributeData.Load();
    }

    public GridGraph GetGridGraph()
    {
        return gridGraph;
    }

    public float GetSpeedMultiplier()
    {
        return speed_multiplier;
    }

    public void SetGameSpeedMultiplier(float mult)
    {
        if (mult > 10) mult = 10;
        if (mult < 0) mult = 0;
        Time.timeScale = mult;
    }

    public bool IsPaused()
    {
        return speed_multiplier < 0.001f;
    }

    public BuildingManager BuildingManager { get { return buildingManager; } }
    public CharacterManager CharacterManager { get { return characterManager; } }
    public GatherableManager GatherableManager { get { return gatherableManager; } }
    public StorageManager StorageManager { get { return storageManager; } }
    public CivilianManager CivilianManager { get { return civilianManager; } }
    public TaskManager TaskManager { get { return taskManager; } }
    public DestructibleManager DestructibleManager { get { return destructManager; } }

    
}
