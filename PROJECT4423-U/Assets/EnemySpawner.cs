using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign this in the Inspector with your enemy prefab
    public float minSpawnInterval = 1f; // Minimum time between each spawn in seconds
    public float maxSpawnInterval = 3f; // Maximum time between each spawn in seconds
    public GameObject player;
    public Creature playerCreature;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCreature = player.GetComponent<Creature>();
        // Start the spawning coroutine
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        // This loop runs for the lifetime of the component/game object
        while (true)
        {
            if (player == null || !player.gameObject.activeInHierarchy)
            {
                break;
            }
            
            // Instantiate a new enemy prefab at the spawner's position
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Randomize the spawn interval within the specified range
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Wait for the randomized interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
