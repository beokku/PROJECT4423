using UnityEngine;
using System.Collections;

public class SpawnerSpawner : MonoBehaviour
{

    [SerializeField] public GameObject prefabToSpawn;
    [SerializeField] public float spawnRadius = 5f;
    [SerializeField] public GameObject player;
    [SerializeField] public float spawnInterval = 2f; // Time in seconds between spawns
    Transform playerTransform;


    private void Start()
    {
        playerTransform = player.transform;
        StartCoroutine(SpawnPrefabOverTime());
    }

    private IEnumerator SpawnPrefabOverTime()
    {
        while (true) // Use with caution, make sure your game has a condition to stop this eventually
        {

            if (player == null || !player.gameObject.activeInHierarchy)
            {
                break; // This exits the coroutine loop
            }

            SpawnPrefabAroundPlayer();
                yield return new WaitForSeconds(spawnInterval);
            
            
        }
    }

    private void SpawnPrefabAroundPlayer()
{
    if (playerTransform == null || prefabToSpawn == null) return;

    // Generate a random angle
    float angle = Random.Range(0, Mathf.PI * 2);

    // Calculate position around the player at a given radius
    Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius;

    // Instantiate the prefab as a child of the player
    GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, playerTransform);
    spawnedObject.transform.localPosition = spawnPosition; // Ensure it's positioned relative to the player
}

 
}
