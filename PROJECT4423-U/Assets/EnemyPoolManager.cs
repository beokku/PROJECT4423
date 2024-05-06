using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour {
    public static EnemyPoolManager Instance; 
    private List<GameObject> pooledEnemies = new List<GameObject>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public GameObject GetPooledEnemy() {
        foreach (var enemy in pooledEnemies) {
            if (!enemy.activeInHierarchy) {
                return enemy;
            }
        }
        return null; // No inactive enemies available
    }

    public void AddToPool(GameObject enemy) {
        if (!pooledEnemies.Contains(enemy)) {
            pooledEnemies.Add(enemy);
        }
    }
}