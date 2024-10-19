using UnityEngine;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float detectionRadius = 9.0f;
    public float speed = 1.5f;
    public int maxRecentWaypoints = 5;
    public float offsetDistance = 0.3f;

    private Transform targetWaypoint;
    private List<Transform> recentWaypoints = new List<Transform>();
    private Vector3 offsetPosition;

    void Start()
    {
        targetWaypoint = GetRandomWaypoint();

        if (targetWaypoint != null)
        {
            offsetPosition = GetOffsetPosition(targetWaypoint.position);
        }
    }

    void Update()
    {
        if (targetWaypoint != null)
        {
            MoveToTarget(offsetPosition);

            if (Vector3.Distance(transform.position, offsetPosition) < 0.8f)
            {
                AddToRecentWaypoints(targetWaypoint);

                targetWaypoint = GetRandomWaypoint();
                if (targetWaypoint != null)
                {
                    offsetPosition = GetOffsetPosition(targetWaypoint.position);
                }
            }
        }
        else
        {
            targetWaypoint = GetRandomWaypoint();
            if (targetWaypoint != null)
            {
                offsetPosition = GetOffsetPosition(targetWaypoint.position);
            }
        }
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void AddToRecentWaypoints(Transform waypoint)
    {
        recentWaypoints.Add(waypoint);

        if (recentWaypoints.Count > maxRecentWaypoints)
        {
            recentWaypoints.RemoveAt(0);
        }
    }

    Transform GetRandomWaypoint()
    {
        List<Transform> availableWaypoints = new List<Transform>();

        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(transform.position, waypoint.position) <= detectionRadius && !recentWaypoints.Contains(waypoint))
            {
                availableWaypoints.Add(waypoint);
            }
        }

        if (availableWaypoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableWaypoints.Count);
            return availableWaypoints[randomIndex];
        }
        else
        {
            return null;
        }
    }

    Vector3 GetOffsetPosition(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        Vector3 perpendicularDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized;

        Vector3 offset = perpendicularDirection * offsetDistance;

        return targetPosition + offset;
    }
}
