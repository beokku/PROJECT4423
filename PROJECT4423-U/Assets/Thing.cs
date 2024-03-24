using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] int health = 3;
    [SerializeField] int damage = 10;
    [SerializeField] int throwForce = 5;

 
    public float moveAmount = 20.0f; // Amount to move on the X axis
    public float moveDuration = 5.0f; // Duration of the move
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTime = 0.0f;

 

    void Update()
    {
        // Increment the time
        currentTime += Time.deltaTime;

        // Calculate the fraction of the journey completed
        float journeyFraction = currentTime / moveDuration;

       

        // Optionally, reset after moving
        if (journeyFraction >= 1)
        {
            currentTime = 0; // Reset time to allow repeating or reversing the lerp
            // Swap start and end positions if you want to move back and forth
            // (Uncomment the following two lines if needed)
            // Vector3 temp = startPosition;
            // startPosition = endPosition; endPosition = temp;
        }
    }

    public void Attack()
    {
        startPosition = transform.position; // Current position
        float journeyFraction = currentTime / moveDuration;
        endPosition = new Vector3(startPosition.x + moveAmount, startPosition.y, startPosition.z);
        // Lerp position
        transform.position = Vector3.Lerp(transform.position, endPosition, 10000 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Enemy") {Destroy(other.gameObject); }
        else { }
    }

}
