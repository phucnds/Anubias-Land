using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lblTaskName;
    [SerializeField] private Button btnIncrease;
    [SerializeField] private Button btnDecrease;
    [SerializeField] private TextMeshProUGUI lblWorkerAmount;
    [SerializeField] private Transform listCivilianAssignedThisTask;
    [SerializeField] private GameObject workerItem;

    private Task task;

    private void Awake()
    {
        btnIncrease.onClick.AddListener(IncreaseWorker);
        btnDecrease.onClick.AddListener(DecreaseWorker);
    }

    public void SetUpItem(Task task)
    {
        if (task == null)
        {
            Debug.Log("Task null");
            return;
        }
        this.task = task;
        lblTaskName.text = task.gameObject.name;
        this.task.OnChangedAssignedWorkers += RefreshUIListView;
    }

    private void IncreaseWorker()
    {
        Debug.Log("IncreaseWorker");
        if (task == null)
        {
            Debug.Log("Task null");
            return;
        }
        int value = task.GetWorkerAmount();
        value++;
        task.SetWorkerAmount(value);
        lblWorkerAmount.text = task.GetWorkerAmount() + "";
        RefreshUIListView();
    }

    private void DecreaseWorker()
    {
        Debug.Log("DecreaseWorker");
        if (task == null)
        {
            Debug.Log("Task null");
            return;
        }
        int value = task.GetWorkerAmount();
        value--;
        task.SetWorkerAmount(value);
        lblWorkerAmount.text = task.GetWorkerAmount() + "";
        RefreshUIListView();
    }

    private void RefreshUIListView()
    {
        Debug.Log("RefreshUIListView");
        foreach (Transform trans in listCivilianAssignedThisTask)
        {
            Destroy(trans.gameObject);
        }

        foreach (Civilian civilian in task.GetAssignedWorkers())
        {
            Instantiate(workerItem, listCivilianAssignedThisTask);
        }
    }

}
