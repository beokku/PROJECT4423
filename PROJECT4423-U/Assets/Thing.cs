using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] int health = 3;
    [SerializeField] int damage = 10;
    [SerializeField] float moveAmount = 3.0f; // Amount to move on the X axis


  
    public float moveDuration = 5.0f; // Duration of the move
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTime = 0.0f;

 

    void Update()
    {
        // Make sure the attack is only triggered once per activation and not every frame during the boost
        if (currentTime <= moveDuration)
        {
            // Increment the time
            currentTime += Time.deltaTime;

            // Calculate the fraction of the journey completed
            float journeyFraction = currentTime / moveDuration;

            // Lerp position towards the endPosition
            transform.position = Vector3.Lerp(startPosition, endPosition, journeyFraction);
        }
    }

    public void Attack()
    {
        startPosition = transform.position; // Current position
        currentTime = 0; // Reset the timer for a smooth start

        // Calculate the end position based on the object's forward vector
        Vector3 forwardDirection = transform.right; // Assuming the "forward" is towards the right
        endPosition = startPosition + forwardDirection * moveAmount;

        // Instantly boost towards the target direction by setting a shorter moveDuration
        moveDuration = 0.1f; // Adjust this value to control the speed of the boost
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Enemy") {Destroy(other.gameObject); }
        
    }
    */
}
