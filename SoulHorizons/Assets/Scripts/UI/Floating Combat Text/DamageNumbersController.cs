using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DamageNumbersController : MonoBehaviour {

	public static DamageNumbersController damageNumbers;
	public DamageNumbers damageNumbersPrefab;
	private DamageNumbers[] numbersCache;
	private DamageNumbers currentNum;
	// Use this for initialization
	void Awake()
	{
		if(damageNumbers != null && damageNumbers != this)
			Destroy(this);
		else
			damageNumbers = this;
			
	}

	void Start () 
	{
		numbersCache = new DamageNumbers[10];
		for(int x = 0; x < 10; x++)
		{
			numbersCache[x] = Instantiate(damageNumbersPrefab.gameObject, transform.position, Quaternion.identity, transform).GetComponent<DamageNumbers>();
		}
	}

	public void SpawnNumbers(string text, Vector3 position, Color color, float scaleMulti)
	{
		currentNum = NumberCache();
		currentNum.SetNumberDetails(text, color, scaleMulti);
		currentNum.transform.position = position;
        currentNum.GetComponent<MeshRenderer>().sortingLayerName = "UI"; 
        currentNum.GetComponent<MeshRenderer>().sortingOrder = 1000000; 

	}

	public void SpawnNumbers(string text, Vector3 position, Color color)
	{
		SpawnNumbers(text, position, color, 1f);
	}

	public void SpawnNumbers(int text, Vector3 position, Color color)
	{
		SpawnNumbers(text.ToString(), position, color);
	}

	DamageNumbers NumberCache()
	{
		foreach(DamageNumbers num in numbersCache)
		{
			if(num.isActive == false)
				return num;
		}
		return numbersCache[0];
	}
}
