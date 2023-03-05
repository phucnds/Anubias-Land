using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private List<Storage> listStorage = new List<Storage>();

    private void Awake() {
        Storage.OnAnyStorageCreated += Storage_OnAnyStorageCreated;
        Storage.OnAnyStorageDestroyed += Storage_OnAnyStorageDestroyed;
    }

    private void Storage_OnAnyStorageCreated(Storage storage)
    {
        listStorage.Add(storage);
    }

    private void Storage_OnAnyStorageDestroyed(Storage storage)
    {
        listStorage.Remove(storage);
    }

    public List<Storage> GetListStorage()
    {
        return listStorage;
    }
}
