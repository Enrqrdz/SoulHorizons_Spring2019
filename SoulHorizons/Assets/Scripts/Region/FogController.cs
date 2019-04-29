using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Needs To Be Set")]
    public List<GameObject> cloudPrefabs;
    public Vector3 topLeftCorner, bottomRightCorner;

    [Header("Options")]
    private int cloudsPerRow = 30;
    private int numberOfRows = 30;
    private float maxForce = 10f;
    private int minmumOrderInLayer = 2;
    private float scaleRange = .2f;

    private List<GameObject> clouds = new List<GameObject>();

    void Start()
    {
        float verticalDistancePerRow = (topLeftCorner.y - bottomRightCorner.y) / numberOfRows;

        for(int i = 0; i < numberOfRows; i++)
        {
            Vector3 force = new Vector3(Random.Range(-maxForce, maxForce), 0, 0);

            for(int j = 0; j < cloudsPerRow; j++)
            {
                int r = Random.Range(0, cloudPrefabs.Count);

                float yPosition = (verticalDistancePerRow * i) + bottomRightCorner.y;

                Vector3 position = new Vector3(Random.Range(topLeftCorner.x, bottomRightCorner.x), yPosition, 0);

                GameObject newCloud = Instantiate(cloudPrefabs[r], position, Quaternion.identity);

                newCloud.GetComponent<Rigidbody2D>().AddForce(force);
                newCloud.GetComponent<SpriteRenderer>().sortingOrder = numberOfRows + minmumOrderInLayer - i;
                float rScale = Random.Range(1 - scaleRange, 1 + scaleRange);
                newCloud.transform.localScale = new Vector3(rScale, rScale, 1);
                newCloud.transform.parent = gameObject.transform;

                clouds.Add(newCloud);
            }
        }
    }

    void Update()
    {

    }
}
