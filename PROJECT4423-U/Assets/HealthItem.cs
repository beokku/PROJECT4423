using UnityEngine;
using System.Collections;

public class HealthItem : MonoBehaviour {
    [SerializeField] private float lifetime = 10f; // Time in seconds before the item destroys itself
    [SerializeField] private float attractionDistance = 5f; // Distance within which the item starts moving towards the player
    [SerializeField] private float moveSpeed = 2f; // Speed at which the item moves towards the player

    private GameObject player;
    private bool isPickedUp = false; // To check if the item has been picked up and avoid unnecessary updates

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DestroyAfterTime());
    }

    private void Update() {
        if (player != null && !isPickedUp) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attractionDistance) {
                // Move the item towards the player
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(lifetime);
        // After waiting, destroy health item if it hasn't been picked up
        if (!isPickedUp) {
            Destroy(gameObject);
        }
    }

    void OnPickedUp() {
        isPickedUp = true;
        StopAllCoroutines();
    }
}
