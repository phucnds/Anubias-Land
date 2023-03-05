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
    [SerializeField] private GameObject uiCanvasPrefab;

    private UIManager uiCanvas;

    public static GameMgr Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        LoadData();
    }

    private void LoadData()
    {
        ActionBasic.Load();

        if(uiCanvas == null) uiCanvas = Instantiate(uiCanvasPrefab,transform).GetComponent<UIManager>();

    }

    public BuildingManager BuildingManager { get { return buildingManager; } }
    public CharacterManager CharacterManager { get { return characterManager; } }
    public GatherableManager GatherableManager { get { return gatherableManager; } }
    public StorageManager StorageManager { get { return storageManager; } }

    public UIManager UICanvas { get { return uiCanvas; } }
}
