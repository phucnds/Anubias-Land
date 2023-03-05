using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private Transform tabButtons;
    [SerializeField] private Transform groupContent;

    private int index = 0;

    private void Awake() {
        foreach(Transform trans in tabButtons)
        {
            Button button = trans.GetComponent<Button>();
            button.onClick.AddListener(() => {
                FilterContent(trans.GetSiblingIndex());
            });
        }

        FilterContent(index);
    }


    private void FilterContent(int index)
    {
        HideAllChild();
        groupContent.GetChild(index).gameObject.SetActive(true);
    }


    private void HideAllChild()
    {
        foreach (Transform trans in groupContent)
        {
            trans.gameObject.SetActive(false);
        }
    }
}
