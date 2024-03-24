using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign this in the Inspector with your enemy prefab
    public float spawnInterval = 2f; // Time between each spawn in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        // This loop runs for the lifetime of the component/game object
        while (true)
        {
            // Instantiate a new enemy prefab at the spawner's position
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}