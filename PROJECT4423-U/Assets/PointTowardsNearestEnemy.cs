using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsNearestEnemy : MonoBehaviour
{
    [SerializeField] private bool pointAtCursor = false; // Flag to decide between pointing at cursor or nearest enemy
    [SerializeField] private float rotationSpeed = 200f; // Control the speed of rotation
    [SerializeField] private float rotationOffset = 0f; // Rotation offset for sprite alignment
    [SerializeField] private float leftOffset = 0f; // Additional left rotation offset



    
    void Update()
    {
        Vector2 targetPosition;

        if (!pointAtCursor)
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

            // If a nearest enemy is found, set the target position to the enemy's position
            if (nearestEnemy != null)
            {
                targetPosition = nearestEnemy.transform.position;
            }
            else
            {
                return; // If no enemies, do nothing
            }
        }
        else
        {
            // Point towards the cursor
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Calculate the direction and target angle based on the target position
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Check if the target is to the left; adjust for sprite angle if needed
        if (direction.x < 0)
        {
            targetAngle -= leftOffset;
        }

        // Apply rotation offset
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle + rotationOffset);

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Determine sprite flipping based on the angle
        if (targetAngle > 90 || targetAngle < -90)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    
    public void toggleAim() {pointAtCursor = !pointAtCursor;}
    public Boolean getAimToggle() {return pointAtCursor;}
}
