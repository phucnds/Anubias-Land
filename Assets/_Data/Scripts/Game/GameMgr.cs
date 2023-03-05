using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private CharacterManager characterManager;

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

    public UIManager UICanvas { get { return uiCanvas; } }
}
