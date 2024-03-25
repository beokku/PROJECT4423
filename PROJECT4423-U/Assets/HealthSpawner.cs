using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthPrefab; // Assign this in the Inspector with your enemy prefab
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void spawnHealth()
    {
        Instantiate(healthPrefab, transform.position, Quaternion.identity);
    }
}
