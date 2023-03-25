using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab = null;

    public void Spawn(float damageAmount)
    {
        DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        instance.SetValue(damageAmount);
    }
}
