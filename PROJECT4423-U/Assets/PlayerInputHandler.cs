using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] Creature playerCreature;
    [SerializeField] Thing weapon;
    [SerializeField] RangedThing rangedWeapon;
    [SerializeField] float attackInterval = 0.5f; // Seconds between attacks
    bool isAttacking = false;

    //ProjectileThrower projectileThrower;

    void Start()
    {
        //projectileThrower = playerCreature.GetComponent<ProjectileThrower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCreature.health > 0)
        {
            Vector3 input = Vector3.zero;

            // if (Input.GetKey(KeyCode.W))
            // {
            //     input.y += 1;
            // }

            // if (Input.GetKey(KeyCode.S))
            // {
            //     input.y += -1;
            // }

            if (Input.GetKey(KeyCode.A))
            {
                input.x += -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }

            if (Input.GetKey(KeyCode.W))
            {
                input.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                input.y -= 1;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerCreature.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //playerCreature.Jump();
            }

            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                StartCoroutine(AttackRepeatedly());
            }
            else if (Input.GetMouseButtonUp(0) && isAttacking)
            {
                StopCoroutine(AttackRepeatedly());
                isAttacking = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //playerCreature.Boost();
            }

            playerCreature.MoveCreature(input);

        }
    }

    IEnumerator AttackRepeatedly()
    {
        isAttacking = true;
        while (isAttacking)
        {
            rangedWeapon.Attack();
            yield return new WaitForSeconds(attackInterval);
        }
    }

}