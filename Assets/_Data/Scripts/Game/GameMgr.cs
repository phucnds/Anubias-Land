using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private GatherableManager gatherableManager;
    [SerializeField] private StorageManager storageManager;

    [Header("UICanvas")]
    [SerializeField] private UIManager uiCanvas;

    public static GameMgr Instance { get; private set; }

    [SerializeField] private float speed_multiplier = 1f;

    private void Awake()
    {
        Instance = this;
        LoadData();
    }

    private void LoadData()
    {
        ActionBasic.Load();
    }

    public float GetSpeedMultiplier()
    {
        return speed_multiplier;
    }

    public void SetGameSpeedMultiplier(float mult)
    {
        if (mult > 10) mult = 10;
        if (mult < 0) mult = 0;
        speed_multiplier = mult;
    }

    public BuildingManager BuildingManager { get { return buildingManager; } }
    public CharacterManager CharacterManager { get { return characterManager; } }
    public GatherableManager GatherableManager { get { return gatherableManager; } }
    public StorageManager StorageManager { get { return storageManager; } }

    public UIManager UICanvas { get { return uiCanvas; } }
}
