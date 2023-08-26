using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HybridPotions", menuName = "Items/HybridPotions")]
public class HybridPotions : Item
{
    [SerializeField] private int effect = 20;

    public int Effect
    {
        get { return effect; }
        set { effect = value; }
    }
}
