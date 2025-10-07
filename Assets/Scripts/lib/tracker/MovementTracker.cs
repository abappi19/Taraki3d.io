using System.Collections.Generic;
using UnityEngine;

public class MovementTracker
{
    private class MovementPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public MovementPoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    private float gapBetweenPoints;
    private float rotateSpeed;
    private List<MovementPoint> movementPoints = new List<MovementPoint>();

    public MovementTracker(float gapBetweenPoints, float rotateSpeed)
    {
        this.gapBetweenPoints = gapBetweenPoints;
        this.rotateSpeed = rotateSpeed;
    }

    public void InsertMovementPoint(Vector3 position, Quaternion rotation)
    {
        movementPoints.Insert(0, new MovementPoint(position, rotation));
    }

    private int? GetPointByGap(Vector3 fromPosition, int startIndex)
    {
        for (int i = startIndex; i < movementPoints.Count; i++)
        {
            if (Vector3.Distance(fromPosition, movementPoints[i].position) > gapBetweenPoints)
                return i;
        }
        return null;
    }

    public void FollowMovement(List<GameObject> objects)
    {

        int index = 0;
        foreach (var obj in objects)
        {
            MovementPoint targetPoint = movementPoints[Mathf.Min(index * 13, movementPoints.Count - 1)];
            Vector3 direction = targetPoint.position - obj.transform.position;
            obj.transform.LookAt(targetPoint.position);
            int speed = 4;
            obj.transform.position += direction * speed * Time.deltaTime;

            index++;
        }


        // clean up unused points
        if (movementPoints.Count > index)
        {
            movementPoints.RemoveRange(index + 1, movementPoints.Count - 1);
        }

        return;
        // if (movementPoints.Count < 2) return;

        // int startIndex = 0;
        // Vector3 prevPos = movementPoints[0].position;
        // for (int i = 0; i < objects.Count; i++)
        // {
        //     GameObject obj = objects[i];
        //     int? foundIndex = GetPointByGap(prevPos, startIndex);
        //     if (foundIndex == null) break;

        //     startIndex = foundIndex.Value; // cache for next body part
        //     MovementPoint targetPoint = movementPoints[startIndex];
        //     obj.transform.position = targetPoint.position;
        //     prevPos = targetPoint.position;

        //     // Quaternion newRot = Quaternion.RotateTowards(
        //     //     obj.transform.rotation,
        //     //     targetPoint.rotation,
        //     //     rotateSpeed * Time.fixedDeltaTime
        //     // );

        //     // obj.transform.SetPositionAndRotation(targetPoint.position, newRot);
        // }

    }
}
