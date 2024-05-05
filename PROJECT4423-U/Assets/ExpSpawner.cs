using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSpawner : MonoBehaviour
{
    public GameObject expPrefab; 
    void Start()
    {
        
    }

    // Update is called once per frame
    public void spawnExp()
    {
        Instantiate(expPrefab, transform.position, Quaternion.identity);
    }
}
