using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A supply of objects to be used as needed in place of creating new objects during gameplay
/// </summary>
public class scr_ObjectPool : MonoBehaviour {

	private List<GameObject> supplyPool = new List<GameObject>(); //the pool of created objects
	public int initialPoolSize = 10; //the number of objects created initially
	public GameObject objectPrefab; //the object being stored

	void Start () {
		for(int i = 0; i < initialPoolSize;i++){
			//add an object to the pool
			GameObject obj = Instantiate(objectPrefab, transform.position,transform.rotation);
			obj.SetActive(false);
			supplyPool.Add(obj);
		}
	}


	public GameObject CreateObject(Vector3 position, Quaternion rotation){
		//get an object from the pool or create a new one if necessary
		GameObject obj = GetObject();
		//position the object
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		//activate the object
		obj.SetActive(true);

		return obj;
	}

    private GameObject GetObject()
    {
        //retrieve an available object from the pool
		foreach (GameObject b in supplyPool)
		{
			if(!b.activeSelf){
				return b;
			}
		}

		//if the pool ran out, create a new object and add it to the pool
		GameObject obj = Instantiate(objectPrefab, transform.position,transform.rotation);
		supplyPool.Add(obj);
		return obj;
    }
}
