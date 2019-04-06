using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Needs To Be Set")]
    public List<GameObject> cloudPrefabs;
    public Vector3 topLeftCorner, bottomRightCorner;

    [Header("Options")]
    private int cloudNumber = 10000;
    private float maxForce = 10f;

    private GameObject[] clouds;

    void Start()
    {
        Debug.Log("test");
        clouds = new GameObject[cloudNumber];

        for(int i = 0; i < clouds.Length; i++)
        {
            int r = Random.Range(0, cloudPrefabs.Count);
            Vector3 position = new Vector3(Random.Range(topLeftCorner.x, bottomRightCorner.x), Random.Range(topLeftCorner.y, bottomRightCorner.y), 0);

            clouds[i] = Instantiate(cloudPrefabs[r], position, Quaternion.identity);

            clouds[i].GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-maxForce, maxForce), 0, 0));

            clouds[i].transform.parent = gameObject.transform;
        }
    }

    void Update()
    {
        
    }
}
