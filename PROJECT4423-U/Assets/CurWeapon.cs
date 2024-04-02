using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurWeapon : MonoBehaviour
{
    [SerializeField] public Creature player;   
    [SerializeField] public Thing currentObject;
    [SerializeField] public RangedThing currentRangedObject;

    public void Update()
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
            transform.position = Vector3.Lerp(transform.position, player.transform.position, 20 * Time.deltaTime);
        }
    }
}