using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedThing : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f; // Control the speed of rotation
    [SerializeField] private float rotationOffset = 0f; // Rotation offset for sprite alignment
    [SerializeField] private GameObject carrotProjectile; // The projectile prefab
    [SerializeField] private float projectileSpeed = 10f; // Speed of the projectile
    [SerializeField] private int numberOfProjectiles = 1; // Number of projectiles to fire
    [SerializeField] private float spreadAngle = 10f; // Angle between each projectile in the spread
    [SerializeField] private float projectileAngleOffset = 0f; // Angle offset for projectile alignment
    [SerializeField] private float maxRandomRotation = 30f; // Max random rotation angle

    void Update()
    {
        // Existing code to rotate towards the nearest enemy
    }

    public void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Calculate the starting angle based on the number of projectiles
            float startAngle = baseAngle - spreadAngle * (numberOfProjectiles - 1) / 2;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                // Add a random rotation to each projectile within the specified range
                float randomRotation = Random.Range(-maxRandomRotation, maxRandomRotation);
                float currentAngle = startAngle + spreadAngle * i + projectileAngleOffset + randomRotation; // Apply random rotation here
                Quaternion projectileRotation = Quaternion.Euler(0f, 0f, currentAngle);
                GameObject projectile = Instantiate(carrotProjectile, transform.position, projectileRotation);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 projectileDirection = projectileRotation * Vector2.right;
                    rb.velocity = projectileDirection * projectileSpeed;
                }
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
