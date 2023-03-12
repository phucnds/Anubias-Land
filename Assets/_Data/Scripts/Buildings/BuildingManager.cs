using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<Building> listBuilding = new List<Building>();

    private Building TownHall;

    private List<Inns> listInns = new List<Inns>();
    private List<Market> listMarket = new List<Market>();
    private List<Shop> listShop = new List<Shop>();
    private List<Fortress> listFortress = new List<Fortress>();

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

        if (building is Inns)
        {
            listInns.Remove((Inns)building);
        }

        if (building is Market)
        {
            listMarket.Remove((Market)building);
        }

        if (building is Shop)
        {
            listShop.Remove((Shop)building);
        }

        if (building is Fortress)
        {
            listFortress.Remove((Fortress)building);
        }
    }

    private void Building_OnAnyBuildingCreated(Building building)
    {
        listBuilding.Add(building);

        if (building is Inns)
        {
            listInns.Add((Inns)building);
        }

        if (building is Market)
        {
            listMarket.Add((Market)building);
        }

        if (building is Shop)
        {
            listShop.Add((Shop)building);
        }

        if (building is Fortress)
        {
            listFortress.Add((Fortress)building);
        }

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

    public List<Inns> GetListInns()
    {
        return listInns;
    }

    public List<Market> GetListMarkets()
    {
        return listMarket;
    }

    public List<Shop> GetListShops()
    {
        return listShop;
    }

    public List<Fortress> GetListFortress()
    {
        return listFortress;
    }
}
