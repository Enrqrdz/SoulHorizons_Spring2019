using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;

    private bool isMoving = false;
    private Queue<Vector3> destinationQueue = new Queue<Vector3>();
    private int lerpTime = 0;
    private float waitTime;
    private bool isWaiting = false;

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, destinationQueue.Peek(), lerpTime * cameraSpeed);
            
            lerpTime ++;

            if(Vector3.Distance(transform.position ,destinationQueue.Peek()) <= .1 && !isWaiting)
            {
                Invoke("OnReachingDestination", waitTime);
                isWaiting = true;
            }
        }
    }

    public void AddDestination(Vector3 newDestination)
    {
        newDestination.z = transform.position.z;
        destinationQueue.Enqueue(newDestination);
        isMoving = true;
        lerpTime = 0;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void OnReachingDestination()
    {
        destinationQueue.Dequeue();
        isMoving = (destinationQueue.Count != 0);
        lerpTime = 0;
        waitTime = 0f;
        isWaiting = false;
    }

    public void SetWaitTime(float newTime)
    {
        waitTime = newTime;
    }
}
