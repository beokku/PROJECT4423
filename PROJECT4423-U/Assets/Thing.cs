using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] public int damage;
    [SerializeField] float moveAmount; 

  
    public float moveDuration = 5.0f; 
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTime = 0.0f;


    void Update() {
        if (currentTime <= moveDuration) {
            currentTime += Time.deltaTime;
            float journeyFraction = currentTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, journeyFraction);
        }
    }

    public void Attack() {
        startPosition = transform.position; 
        currentTime = 0; 
        Vector3 forwardDirection = transform.right; 
        endPosition = startPosition + forwardDirection * moveAmount;
        moveDuration = 0.1f;
    }
}
