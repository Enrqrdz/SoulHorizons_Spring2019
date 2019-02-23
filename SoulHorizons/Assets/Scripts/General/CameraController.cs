using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;

    private bool isMoving = false;
    private Vector3 destination;
    private int lerpTime = 0;

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, destination, lerpTime * cameraSpeed);
            lerpTime ++;

            if(transform.position == destination)
            {
                isMoving = false;
                lerpTime = 0;
            }
        }
    }

    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
        isMoving = true;
        lerpTime = 0;
    }
}
