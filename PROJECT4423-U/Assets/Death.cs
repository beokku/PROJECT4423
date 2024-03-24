using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {

        //GetComponent<SpriteRenderer>().color = Color.blue;
        Destroy(this.gameObject);
        //other.GetComponent<SpriteRenderer>().color = Color.red;

    }
}
