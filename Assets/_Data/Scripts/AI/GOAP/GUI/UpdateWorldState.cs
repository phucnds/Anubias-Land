using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateWorldState : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    private void LateUpdate()
    {
        Dictionary<string, int> worldstates = GWorld.Instance.GetWorld().GetStates();
        text.text = "";
        foreach (KeyValuePair<string, int> s in worldstates)
        {
            text.text += s.Key + ": " + s.Value + "\n";
        }
    }
}
