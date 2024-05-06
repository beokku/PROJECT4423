using UnityEngine;
using System.Collections;

public class SpawnerSpawner : MonoBehaviour {

    [SerializeField] public GameObject prefabToSpawn;
    [SerializeField] public float spawnRadius = 5f;
    [SerializeField] public GameObject player;
    [SerializeField] public float spawnInterval = 2f; 
    Transform playerTransform;


    private void Start() {
        playerTransform = player.transform;
        StartCoroutine(SpawnPrefabOverTime());
    }

    private IEnumerator SpawnPrefabOverTime() {
        while (true) {
            if (player == null || !player.gameObject.activeInHierarchy) {
                break; // This exits the coroutine loop
            }

            SpawnPrefabAroundPlayer();
                yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPrefabAroundPlayer() {
    if (playerTransform == null || prefabToSpawn == null) return;

    float angle = Random.Range(0, Mathf.PI * 2);
    Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius;
    GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, playerTransform);
    spawnedObject.transform.localPosition = spawnPosition; // Ensure it's positioned relative to the player
}

 
}
