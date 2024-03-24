using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiscreteMovement : MonoBehaviour
{

    [SerializeField] float speed = 8.0f;


    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    /*public void PickupCoin()
    {
        GetComponent<AudioSource>().pitch = (Random.Range(1f, 1.5f));
        GetComponent<AudioSource>().Play();
    }*/
}