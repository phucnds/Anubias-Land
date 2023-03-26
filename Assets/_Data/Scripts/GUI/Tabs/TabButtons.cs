using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabButtons : MonoBehaviour
{
    
    public Button tabBtn;
    public TextMeshProUGUI lblTitle;


    private void Awake()
    {
        tabBtn = GetComponent<Button>();
        lblTitle = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetActive()
    {
        lblTitle.color = Color.white;
    }
}
