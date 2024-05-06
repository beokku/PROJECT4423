using UnityEngine;
using System.Collections;

public class ExpItem : MonoBehaviour
{
    const float lifetime = 10f; 
    const float attractionDistance = 5f; 
    const float attractionSpeed = 2f;



    private GameObject player;
    private bool isPickedUp = false; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (player != null && !isPickedUp)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attractionSpeed)
            {
                // Move the item towards the player
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attractionSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        if (!isPickedUp)
        {
            Destroy(gameObject);
        }
    }

    void OnPickedUp()
    {
        isPickedUp = true;
        StopAllCoroutines();
    }
}
