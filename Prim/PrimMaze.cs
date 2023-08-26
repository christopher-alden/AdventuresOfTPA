using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimMaze : MonoBehaviour
{
    public GenerateBSP bspMap;
    public GameObject obstacle;
    public LayerMask SpawnMask;
    public Vector2 gridWorldSize;
    public int gridSizeX, gridSizeY;
    PrimNode[,] grids;
    public float nodeRadius;
    public float nodeDiameter;
    public bool displayPrimGizmos;
    float nodeDistance = 0.5f;

    private List<BoundsInt> bspRoom;

    public PrimNode[,] Grids
    {
        get { return grids; }
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeDiameter + nodeDistance));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (nodeDiameter + nodeDistance));
        bspRoom = bspMap.RoomList;
        obstacle.layer = LayerMask.NameToLayer("Unwalkable");
        createGrid();
        DFS();
        
    }

    void createGrid()
    {
        grids = new PrimNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - new Vector3(gridWorldSize.x / 2, 0, gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 gridPosition = worldBottomLeft + Vector3.right * (x * (nodeDiameter + nodeDistance) + nodeRadius) + Vector3.forward * (y * (nodeDiameter + nodeDistance) + nodeRadius);
                Vector3 worldPosition = new Vector3(gridPosition.x, 20, gridPosition.z);
                bool spawnable = false;
                RaycastHit hit;
                if (Physics.Raycast(worldPosition, Vector3.down, out hit))
                {
                    if (hit.collider.CompareTag("Ground") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Unwalkable"))
                    {
                        spawnable = true;
                    }
                }

                grids[x, y] = new PrimNode(spawnable, worldPosition, x, y);
            }
        }
    }

    void DFS()
    {
        PrimNode startNode = grids[0, 0];
        startNode.isVisited = true;

        Stack<PrimNode> nodeStack = new Stack<PrimNode>();
        nodeStack.Push(startNode);

        while (nodeStack.Count > 0)
        {
            PrimNode current = nodeStack.Pop();

            List<PrimNode> neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                nodeStack.Push(current);

                int index = Random.Range(0, neighbors.Count);
                PrimNode selectedNeighbor = neighbors[index];

                selectedNeighbor.isVisited = true;
                selectedNeighbor.parent = current;
                nodeStack.Push(selectedNeighbor);

                bool inARoom = IsInsideRoom(selectedNeighbor);
                if (inARoom)
                {
                    Vector3 spawnPosition = selectedNeighbor.worldPosition;
                    SpawnObject(spawnPosition);
                }
            }
        }
    }

    bool IsInsideRoom(PrimNode node)
    {
        foreach (BoundsInt room in bspRoom)
        {
            Vector3 nodePosition = node.worldPosition;
            if (room.Contains(new Vector3Int(Mathf.RoundToInt(nodePosition.x), Mathf.RoundToInt(nodePosition.y), Mathf.RoundToInt(nodePosition.z))))
            {
                return true;
            }
        }
        return false;
    }

    void SpawnObject(Vector3 position)
    {
        if (Random.value < 0.25f)
        {
            int boxXPosition = Mathf.RoundToInt(position.x);
            int boxZPosition = Mathf.RoundToInt(position.z);

            Vector3 boxPosition = new Vector3(boxXPosition, position.y, boxZPosition);

            RaycastHit hit;
            if (Physics.Raycast(boxPosition, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Ground") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Unwalkable"))
                {
                    boxPosition.y = hit.point.y + 1f;
                    GameObject spawnedObstacle = Instantiate(obstacle, boxPosition, Quaternion.identity);

                    Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                    spawnedObstacle.transform.position += offset;

                    var scaleDifference = Random.Range(0.8f, 1.2f);
                    spawnedObstacle.transform.localScale *= scaleDifference;

                    var range = Random.Range(-45f, 45f);
                    Quaternion randomRotation = Quaternion.Euler(range, range, range);
                    spawnedObstacle.transform.rotation = randomRotation;
                }
            }
        }
    }

    List<PrimNode> GetUnvisitedNeighbors(PrimNode node)
    {
        List<PrimNode> neighbors = new List<PrimNode>();
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        for (int d = 0; d < 4; d++)
        {
            int nx = node.GridX + dx[d];
            int ny = node.GridY + dy[d];

            if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
            {
                PrimNode neighborNode = grids[nx, ny];

                if (!neighborNode.isVisited)
                {
                    neighbors.Add(neighborNode);
                }
            }
        }

        return neighbors;
    }

    private void OnDrawGizmos()
    {
        if (grids != null && displayPrimGizmos)
        {
            foreach (PrimNode pn in grids)
            {
                //Gizmos.color = Color.cyan;
                //Gizmos.DrawWireCube(pn.worldPosition, Vector3.one * (nodeDiameter - 1f));

                if (pn.parent != null)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(pn.worldPosition, pn.parent.worldPosition);
                }
            }
        }
    }
}

public class PrimNode
{
    public bool walkable;
    public Vector3 worldPosition;
    public PrimNode parent;
    public bool isVisited;
    public int GridX, GridY;

    public PrimNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        GridX = _gridX;
        GridY = _gridY;
        parent = null;
        isVisited = false;
    }
}
