using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana Potion", menuName = "Items/ManaPotion")]
public class ManaPotion : Item
{ 

    [SerializeField] private int mana = 10;

    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

}
