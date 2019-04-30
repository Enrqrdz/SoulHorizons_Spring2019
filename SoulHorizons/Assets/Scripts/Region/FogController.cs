using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Needs To Be Set")]
    public List<GameObject> cloudRows;
    public Vector3 topLeftCorner, bottomRightCorner;

    [Header("Options")]
    private float maxForce = 10f;

    void Start()
    {
        int forceDirection = 1;

        foreach(GameObject cloudRow in cloudRows)
        {
            float rForce = Random.Range(0, maxForce);
            Vector2 force = new Vector2(rForce * forceDirection, 0);
            AddForceToRow(cloudRow, force);
            forceDirection *= -1;
        }
    }

    void Update()
    {

    }

    void AddForceToRow(GameObject cloudRow, Vector2 force)
    {
        foreach(Transform cloudRibbon in cloudRow.transform)
        {
            foreach(Transform cloud in cloudRibbon.transform)
            {
                cloud.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }
}
