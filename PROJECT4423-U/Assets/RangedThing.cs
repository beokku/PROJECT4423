using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedThing : MonoBehaviour
{
    [SerializeField] private GameObject carrotProjectile;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Creature player;
    [SerializeField] private int numberOfProjectiles = 1;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float accuracy = 0f;
    [SerializeField] private float maxRandomRotation = 1f;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float distanceFromPlayer = 1f; // Distance from the player where projectiles instantiate
    //[SerializeField] private bool aimAtCursor = false; // Flag to decide whether to aim at the cursor

    private Boolean aimAtCursor = false;
    public void Attack()
    {
        Vector2 targetPosition = Vector2.zero;
        aimAtCursor = GetComponent<PointTowardsNearestEnemy>().getAimToggle();

        if (!aimAtCursor)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                targetPosition = nearestEnemy.transform.position;
            }
            else
            {
                return; // No enemy to attack
            }
        }
        else
        {
            // Aim at the cursor's position
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - spreadAngle * (numberOfProjectiles - 1) / 2;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float randomRotation = UnityEngine.Random.Range(-maxRandomRotation, maxRandomRotation);
            float currentAngle = startAngle + spreadAngle * i + accuracy + randomRotation;
            float fullRandomZRotation = UnityEngine.Random.Range(0f, 360f);
            Quaternion projectileRotation = Quaternion.Euler(0f, 0f, currentAngle);
            Quaternion additionalRandomRotation = Quaternion.Euler(0f, 0f, fullRandomZRotation);
            
            // Calculate the starting position based on distanceFromPlayer
            Vector2 startPosition = transform.position + (Vector3)(projectileRotation * Vector2.right * distanceFromPlayer);
            
            GameObject projectile = Instantiate(carrotProjectile, startPosition, projectileRotation * additionalRandomRotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 projectileDirection = projectileRotation * Vector2.right;
                rb.velocity = projectileDirection * projectileSpeed;
            }
        }
        GetComponent<AudioSource>().Play();
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

    public float getAttackSpeed() { return attackSpeed; }
    public void setAttackSpeed(float percentage) {
        attackSpeed += attackSpeed * percentage; // Adjust attack speed by a percentage
        Debug.Log("Attack Speed Changed");
    }

    public void increaseProjectiles(int num) {numberOfProjectiles += num;}
}
