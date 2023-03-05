using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TownHallUI : MonoBehaviour
{
    [SerializeField] private Transform contentView;
    [SerializeField] private GameObject characterItem;

    private void Awake()
    {
        TownHall.OnAnyCharacterStateChanged += TownHall_OnAnyCharacterStateChanged;
    }

    private void Start()
    {
        foreach (Transform trans in contentView)
        {
            Destroy(trans.gameObject);
        }
    }

    private void TownHall_OnAnyCharacterStateChanged()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform trans in contentView)
        {
            Destroy(trans.gameObject);
        }

        foreach (Character character in GameMgr.Instance.BuildingManager.GetTownHall().GetCharactersInTown())
        {
            GameObject row = Instantiate(characterItem, contentView);
        }
    }
}
