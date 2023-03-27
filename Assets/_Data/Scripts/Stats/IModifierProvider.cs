using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifiers(Stat stat);
    IEnumerable<float> GetPercentageModifiers(Stat stat);
}
