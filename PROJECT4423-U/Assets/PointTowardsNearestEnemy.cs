using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsNearestEnemy : MonoBehaviour {
    [SerializeField] private bool pointAtCursor = false; 
    [SerializeField] private float rotationSpeed = 200f; 
    [SerializeField] private float rotationOffset = 0f; 
    [SerializeField] private float leftOffset = 0f; 

    void Update() {
        Vector2 targetPosition;

        if (!pointAtCursor) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject nearestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies) {     // Iterate through all enemies to find the nearest one
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null){
                targetPosition = nearestEnemy.transform.position;
            } else {
                return; 
            }
        } else {
            // Point towards the cursor
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector2 direction = targetPosition - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Check if the target is to the left; adjust for sprite angle if needed
        if (direction.x < 0) {
            targetAngle -= leftOffset;
        }
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle + rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);      // Lerp the change
        if (targetAngle > 90 || targetAngle < -90) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    
    public void toggleAim() {pointAtCursor = !pointAtCursor;}
    public Boolean getAimToggle() {return pointAtCursor;}
}
