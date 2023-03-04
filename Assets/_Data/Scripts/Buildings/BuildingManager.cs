using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<Building> listBuilding = new List<Building>();

    private Building TownHall;


    private void Awake()
    {
        Building.OnAnyBuildingCreated += Building_OnAnyBuildingCreated;
        Building.OnAnyBuildingDetroyed += Building_OnAnyBuildingDetroyed;
    }

    private void Start()
    {

    }

    private void Building_OnAnyBuildingDetroyed(Building building)
    {
        listBuilding.Remove(building);
    }

    private void Building_OnAnyBuildingCreated(Building building)
    {
        listBuilding.Add(building);
    }



    public List<Building> GetBuildingList()
    {
        return listBuilding;
    }

    public void SetTownHall(TownHall townHall)
    {
        this.TownHall = townHall;
    }

    public TownHall GetTownHall()
    {
        return TownHall is TownHall ? (TownHall)TownHall : null;
    }
}
