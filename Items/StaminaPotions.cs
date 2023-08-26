using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaminaPotions", menuName = "Items/StaminaPotions")]
public class StaminaPotions : Item
{
    [SerializeField] private int effect = 20;

    public int Effect
    {
        get { return effect; }
        set { effect = value; }
    }
}
