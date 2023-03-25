using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDamage;

    public void DestroyText()
    {
        Destroy(gameObject);
    }

    public void SetValue(float amount)
    {
        textDamage.text = String.Format("{0:0}", amount);
    }
}
