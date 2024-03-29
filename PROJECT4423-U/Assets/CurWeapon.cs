using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurWeapon : MonoBehaviour
{
    [SerializeField] public Creature player; //Drag the "player" GO here in the Inspector    
    [SerializeField] public Thing currentObject;

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