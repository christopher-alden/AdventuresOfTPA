using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private static Inventory instance;
    private int capacity = 8;
    private int money = 50000;


    #region Singleton
    public static Inventory Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int Money
    {
        get { return money; }
        set { money = value; }
    }
    public int Capacity
    {
        get { return capacity; }
        set { capacity = value; }
    }

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        if(items.Count >= capacity)
        {
            //Debug.Log("Inventory is full");
            return;
        }
        if(item.Price > money)
        {
            //Debug.Log("Ur Broke");
            return;
        }
        items.Add(item);


        if (onItemChangedCallback!=null)onItemChangedCallback.Invoke();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }   

    public int GetCurrentCapacity()
    {
        return items.Count;
    }


}

