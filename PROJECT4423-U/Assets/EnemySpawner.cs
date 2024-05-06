using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign this in the Inspector
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            GameObject enemyToSpawn = EnemyPoolManager.Instance.GetPooledEnemy();
            if (enemyToSpawn != null)
            {
                enemyToSpawn.transform.position = transform.position;
                enemyToSpawn.SetActive(true);
            }
            else
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }

            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
