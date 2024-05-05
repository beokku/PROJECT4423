using UnityEngine;
using System.Collections;

public class TutorialSpriteFade : MonoBehaviour
{
    public SpriteRenderer tutorialSprite1;
    public SpriteRenderer tutorialSprite2;
    public float displayTime = 5.0f; // Time in seconds each sprite is displayed before fading
    public float fadeDuration = 2.0f; // Duration of the fade effect

    IEnumerator Start()
    {
        // Display the sprites for a certain time
        tutorialSprite1.gameObject.SetActive(true);
        tutorialSprite2.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(displayTime);

        // Start fading the sprites
        StartCoroutine(FadeSprite(tutorialSprite1));
        StartCoroutine(FadeSprite(tutorialSprite2));
    }

    IEnumerator FadeSprite(SpriteRenderer spriteRenderer)
    {
        float elapsed = 0.0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.gameObject.SetActive(false);
    }
}
