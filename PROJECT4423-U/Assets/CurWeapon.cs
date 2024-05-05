using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurWeapon : MonoBehaviour
{
    [SerializeField] public Creature player;   
    [SerializeField] public Thing currentObject;
    [SerializeField] public RangedThing currentRangedObject;
    [SerializeField] private float offsetDistance = 1f; // Horizontal distance from the player
    [SerializeField] private float verticalOffset = 1f; // Vertical offset from the player

    void Update()
    {
        CheckGameObject();
    }

    void CheckGameObject()
    {
        if (!player.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            // Calculate new position with horizontal and vertical offset
            Vector3 offsetPosition = player.transform.position + (player.transform.forward * offsetDistance) + new Vector3(0, verticalOffset, 0);
            transform.position = Vector3.Lerp(transform.position, offsetPosition, 20 * Time.deltaTime);

            // Adjust the position of the ranged object if it's not null
            if (currentRangedObject != null)
            {
                Vector3 rangedOffsetPosition = player.transform.position + (player.transform.forward * offsetDistance) + new Vector3(0, verticalOffset, 0);
                currentRangedObject.transform.position = Vector3.Lerp(currentRangedObject.transform.position, rangedOffsetPosition, 20 * Time.deltaTime);
            }
        }
    }

    public void setWeapon(RangedThing weapon) {
        currentRangedObject = weapon;
    }
}
