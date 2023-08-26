using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotions", menuName = "Items/HealthPotions")]
public class HealthPotions : Item
{
    [SerializeField] private int effect = 20;

    public int Effect
    {
        get { return effect; }
        set { effect = value; }
    }
}
