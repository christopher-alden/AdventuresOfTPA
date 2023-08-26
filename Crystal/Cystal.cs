using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cystal : MonoBehaviour
{
    private float hp;

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }
    void Start()
    {
        hp = 1500f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
