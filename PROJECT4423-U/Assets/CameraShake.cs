using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // random direction
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the offsets to the camera's position
            // Since it's a 2D top-down game, we keep the camera's original Z position unchanged
            transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Reset the camera's position to its original location after the shaking has finished.
        transform.localPosition = originalPos;
    }
}
