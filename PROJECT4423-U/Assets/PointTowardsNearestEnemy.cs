using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsNearestEnemy : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 200f; // Control the speed of rotation

    void Update()
    {
        // Find all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Iterate through all enemies to find the nearest one
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // If a nearest enemy is found, smoothly rotate to face the nearest enemy
        if (nearestEnemy != null)
        {
            Vector2 direction = nearestEnemy.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle + 90); // Adjusted angle with additional rotation
            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
