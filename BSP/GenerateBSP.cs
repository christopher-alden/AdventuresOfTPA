using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBSP : MonoBehaviour
{

    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int spaceWidth = 20, spaceHeight = 20;
    [SerializeField] private GameObject myPrefab;

    private List<BoundsInt> roomList;

    public List<BoundsInt> RoomList
    {
        get { return roomList; }
    }

    void Awake()
    {
        Vector3Int position = Vector3Int.FloorToInt(transform.position - new Vector3(spaceWidth / 2, 0, spaceHeight / 2));
        Vector3Int size = new Vector3Int(spaceWidth, 1, spaceHeight);

        BoundsInt initialSpace = new BoundsInt(position, size);

        roomList = BSP.BinarySpacePartitioning(initialSpace, minRoomWidth, minRoomHeight);

    }


    void OnDrawGizmos()
    {
        if (roomList == null)
        {
            return;
        }
        else Gizmos.color = Color.red;
        foreach (var room in roomList)
        {
            Vector3 roomSize = new Vector3(room.size.x, 1, room.size.z);
            Gizmos.DrawWireCube(room.center, roomSize);
        }
    }

}


