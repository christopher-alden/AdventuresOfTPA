using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    PathRequestManager requestManager;
    //public Transform seeker, target;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    //public void SetTarget(Transform mainCharacter)
    //{
    //    target = mainCharacter.transform;
    //}

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closeSet.Contains(neighbour)) continue;
                int newCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = getDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            wayPoints = Retrace(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] Retrace(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplePath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplePath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int getDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
