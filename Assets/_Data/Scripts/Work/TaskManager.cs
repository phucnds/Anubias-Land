using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> listTask = new List<Task>();

    private void Awake()
    {
        Task.OnAnyTaskCreate += Task_OnAnyTaskCreate;
        Task.OnAnyTaskDestroy += Task_OnAnyTaskDestroy;
    }

    private void Task_OnAnyTaskCreate(Task task)
    {
        listTask.Add(task);
    }

    private void Task_OnAnyTaskDestroy(Task task)
    {
        listTask.Remove(task);
    }

    public List<Task> ListTask()
    {
        return listTask;
    }
}
