using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform contentView;
    [SerializeField] private GameObject taskItemPrefab;

    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform trans in contentView)
        {
            Destroy(trans.gameObject);
        }

        foreach (Task task in GameMgr.Instance.TaskManager.ListTask())
        {
            GameObject row = Instantiate(taskItemPrefab, contentView);
            TaskItem taskItem = row.GetComponent<TaskItem>();
            taskItem.SetUpItem(task);
        }
    }
}
