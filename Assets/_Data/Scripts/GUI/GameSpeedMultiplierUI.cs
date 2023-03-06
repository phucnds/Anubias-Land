using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSpeedMultiplierUI : MonoBehaviour
{
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnPrevious;
    [SerializeField] private TextMeshProUGUI gameSpeed;


    private void Awake()
    {
        btnNext.onClick.AddListener(IncreaseSpeed);
        btnPrevious.onClick.AddListener(DecreaseSpeed);
        gameSpeed.text = "x" + GameMgr.Instance.GetSpeedMultiplier();
    }

    private void IncreaseSpeed()
    {
        float speed = GameMgr.Instance.GetSpeedMultiplier();
        GameMgr.Instance.SetGameSpeedMultiplier(speed + 1);
        gameSpeed.text = "x" + GameMgr.Instance.GetSpeedMultiplier();
    }

    private void DecreaseSpeed()
    {
        float speed = GameMgr.Instance.GetSpeedMultiplier();
        GameMgr.Instance.SetGameSpeedMultiplier(speed - 1);
        gameSpeed.text = "x" + GameMgr.Instance.GetSpeedMultiplier();
    }

}
