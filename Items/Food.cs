using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Food")]
public class Food : Item
{
    [SerializeField] private int debuff = 10;

    public int Debuff
    {
        get { return debuff; }
        set { debuff = value; }
    }

}
