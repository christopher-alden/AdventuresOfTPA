using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cam;
    private Transform parent;
    public float yOffset;
    Vector3 offset;

    public float YOffset
    {
        set { yOffset = value; }
    }
    private void Start()
    {
        cam = Camera.main.transform;
        parent = transform.parent;
        offset = new Vector3(0, yOffset, 0);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        transform.position = parent.position + offset;
    }

   
}
