using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCameraSequence : MonoBehaviour
{
    void Start()
    {
        if(GameObject.FindObjectsOfType<BossCameraSequence>().Length > 1)
        {
            Destroy(this);  
            return;
        }
        
        RegionManager regionManager = GameObject.Find("RegionManager").GetComponent<RegionManager>();
        regionManager.BossCameraSequence();
        DontDestroyOnLoad(this);
    }
}
