using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] float lifetime;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        StartCoroutine(ShrinkAndDestroy(lifetime));
    }

    IEnumerator ShrinkAndDestroy(float duration)
    {
        float elapsed = 0; // Track the elapsed time

        while (elapsed < duration)
        {
            float scale = 1.0f - elapsed / duration; // Calculate the scaling factor
            transform.localScale = originalScale * scale; // Apply the scaling

            elapsed += Time.deltaTime; // Increment the elapsed time by the time passed since the last frame
            yield return null; // Wait until the next frame
        }

        Destroy(gameObject); // Destroy the projectile once the coroutine completes
    }
}
