using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TownHallUI townHallUI;

    public TownHallUI TownHallUI { get { return townHallUI; } }
}
