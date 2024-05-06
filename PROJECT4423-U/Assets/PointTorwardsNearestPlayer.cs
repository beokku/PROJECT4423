using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsNearestPlayer : MonoBehaviour {
    [SerializeField] public float rotationSpeed = 200f; 
    [SerializeField] public float moveSpeed = 5f; 

    void Update() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float closestDistance = Mathf.Infinity;

        // Iterate through all players to find the nearest one
        foreach (GameObject player in players) {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                nearestPlayer = player;
            }
        }

        // If a nearest player is found, smoothly rotate to face the nearest player and move towards them
        if (nearestPlayer != null) {
            Vector2 direction = nearestPlayer.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle + 90); // Adjusted angle with additional rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0);
        }
    }
}
