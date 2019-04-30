using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DamageNumbersController : MonoBehaviour 
{
	public static DamageNumbersController Instance;
    public Color regularDamageColor;
    public Color bigDamageColor;
    public float critScale;
	public DamageNumbers damageNumbersPrefab;
	private DamageNumbers[] numbersCache;
	private DamageNumbers currentNum;

	// Use this for initialization
	void Awake()
	{
		if(Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;
			
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

	public void SpawnNormalDamage(string text, Vector3 position)
	{
		SpawnNumbers(text, position, regularDamageColor, 0.75f);
	}

    public void SpawnBigDamage(string text, Vector3 position)
	{
		SpawnNumbers(text, position, bigDamageColor, critScale);
	}

	public void SpawnNumbers(int damage, Vector3 position)
	{
        if(damage == 0)
        {
            return;
        }
        else if(damage <= 10)
        {
            SpawnNormalDamage(damage.ToString(), position);
        }
        else
        {
            SpawnNormalDamage(damage.ToString(), position);
        }
		
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
