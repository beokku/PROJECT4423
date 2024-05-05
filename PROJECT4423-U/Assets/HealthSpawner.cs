using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthPrefab; 
    void Start()
    {
        
    }

    // Update is called once per frame
    public void spawnHealth()
    {
        Instantiate(healthPrefab, transform.position, Quaternion.identity);
    }
}
