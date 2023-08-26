using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Items")]
public abstract class Item : ScriptableObject
{
    [SerializeField] new private string name = "New Item";
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int price = 0;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }


}
