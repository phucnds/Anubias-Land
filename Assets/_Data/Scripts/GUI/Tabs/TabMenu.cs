using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    [SerializeField] private Transform tabButtons;
    [SerializeField] private Transform groupContent;

    private int index = 1;

    private void Awake()
    {
        foreach (Transform trans in tabButtons)
        {
            TabButtons tabButtons = trans.GetComponent<TabButtons>();
            tabButtons.tabBtn.onClick.AddListener(() =>
            {
                FilterContent(trans.GetSiblingIndex());
            });
        }

        tabButtons.GetChild(index).GetComponent<TabButtons>().lblTitle.color = selectedColor;
        FilterContent(index);
    }


    private void FilterContent(int index)
    {
        HideAllChild();
        SetNormalColor();
        groupContent.GetChild(index).gameObject.SetActive(true);
        tabButtons.GetChild(index).GetComponent<TabButtons>().lblTitle.color = selectedColor;
    }


    private void HideAllChild()
    {
        foreach (Transform trans in groupContent)
        {
            trans.gameObject.SetActive(false);
        }
    }

    private void SetNormalColor()
    {
        foreach (Transform trans in tabButtons)
        {
            TabButtons tabButtons = trans.GetComponent<TabButtons>();
            tabButtons.lblTitle.color = normalColor;
        }
    }
}
