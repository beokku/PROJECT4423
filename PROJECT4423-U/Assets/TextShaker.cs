using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShaker : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = rectTransform.anchoredPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            rectTransform.anchoredPosition = originalPos + new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        rectTransform.anchoredPosition = originalPos; // Reset to original position
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
