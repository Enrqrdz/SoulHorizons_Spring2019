using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCamera : MonoBehaviour
{
    void Start()
    {
        float cameraX = GridGenerator.gridCenter.x;
        float cameraY = GridGenerator.gridCenter.y;
        gameObject.transform.position = new Vector3(cameraX, cameraY, gameObject.transform.position.z);
    }
}
