using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    private Transform target;
    float speed = 0.3f;
    Vector3[] path;
    int targetIdx;
    //movement
    private float separationRadius = 2.5f;
    private Vector3 smoothDirectionVelocity;
    private string antiCollisionTag;
    //time
    private float pathUpdateInterval = 0.7f;
    private float timeSinceLastUpdate;
    //condition
    private bool enableUpdate;
    private Vector3 lastTargetPosition;
    private float pathUpdateMoveThreshold = 0.5f;


    public bool EnableUpdate
    {
        get { return enableUpdate; }
        set { enableUpdate = value; }
    }

    public Transform Target
    {
        set { target = value; }
        get { return target; }
    }

    public string AntiCollisionTag
    {
        set { antiCollisionTag = value; }
        get { return antiCollisionTag; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private void Update()
    {
        if (target == null || !enableUpdate) return;

        timeSinceLastUpdate += Time.deltaTime;
        bool shouldUpdatePath = timeSinceLastUpdate > pathUpdateInterval;
        bool targetMoved = Vector3.Distance(target.position, lastTargetPosition) > pathUpdateMoveThreshold;

        if (targetMoved && shouldUpdatePath)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            lastTargetPosition = target.position;
            timeSinceLastUpdate = 0f;
        }
    }



    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (this != null && pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        while (targetIdx < path.Length)
        {
            Vector3 currentWaypoint = path[targetIdx];
            while (Vector3.Distance(transform.position, currentWaypoint) > 0.01f)
            {
                Vector3 separation = Vector3.zero;
                Collider[] hits = Physics.OverlapSphere(transform.position, separationRadius);
                foreach (Collider hit in hits)
                {
                    if (hit.transform != this.transform && hit.gameObject.CompareTag(antiCollisionTag))
                    {
                        Vector3 difference = transform.position - hit.transform.position;
                        difference = difference.normalized / difference.magnitude;
                        separation += difference;
                    }
                }

                float separationWeight = 3f;
                Vector3 direction = (currentWaypoint - transform.position).normalized + separation * separationWeight;
                direction = direction.normalized;

                Vector3 targetPosition = transform.position + direction;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothDirectionVelocity, speed);

                yield return null;
            }
            targetIdx++;
        }
    }

    public Vector3 GetLookAt()
    {
        if (target != null)
        {
            return (target.position - transform.position).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIdx; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);

                if (i == targetIdx)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
