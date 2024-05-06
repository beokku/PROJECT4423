using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] public int damage;
    [SerializeField] float lifetime;
    private Vector3 originalScale;

    void Start() {
        originalScale = transform.localScale; 
        StartCoroutine(ShrinkAndDestroy(lifetime));
    }

    IEnumerator ShrinkAndDestroy(float duration) {
        float elapsed = 0; 

        while (elapsed < duration) {
            float scale = 1.0f - elapsed / duration; 
            transform.localScale = originalScale * scale; 

            elapsed += Time.deltaTime; 
            yield return null; 
        }

        Destroy(gameObject); 
    }
}
