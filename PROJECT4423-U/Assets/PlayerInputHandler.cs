using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Creature playerCreature;
    [SerializeField] private Thing weapon;
    [SerializeField] private RangedThing rangedWeapon;
    [SerializeField] private SpriteRenderer playerSpriteRenderer; // References the SpriteRenderer on the player
    [SerializeField] private Sprite normalSprite; // Normal state sprite
    [SerializeField] private Sprite firingSprite; // Firing state sprite
    [SerializeField] private float firingSpriteDuration = 0.1f; // Duration to show firing sprite
    
    private bool isAttacking = false;
    private bool autoFire = false; // Flag to toggle auto-fire on and off
    private float lastAttackTime = 0f; // Track the time when the last attack was made

    void Update()
    {
        if (playerCreature.health > 0)
        {
            Vector3 input = Vector3.zero;

            if (Input.GetKey(KeyCode.Escape))
            {
                PauseMenuHandler pause = FindObjectOfType<PauseMenuHandler>();
                pause.Pause();
            }
            
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
                playerCreature.getCurrentWeapon().GetComponent<PointTowardsNearestEnemy>().toggleAim();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //playerCreature.Jump();
            }

            // Toggle auto-fire with the 'E' key
            if (Input.GetKeyDown(KeyCode.E))
            {
                autoFire = !autoFire; // Toggle auto-fire state
            }

            // Handle attack input for both auto-fire and manual fire
            if (Input.GetMouseButton(0) || autoFire)
            {
                float attackSpeed = rangedWeapon.getAttackSpeed();
                if (Time.time >= lastAttackTime + 1f / attackSpeed)
                {
                    if (!isAttacking)
                    {
                        StartCoroutine(AttackRepeatedly());
                    }
                }
            }
            else if (isAttacking)
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
            if (Time.time >= lastAttackTime + 1f / rangedWeapon.getAttackSpeed())
            {
                playerSpriteRenderer.sprite = firingSprite; // Change to the firing sprite
                rangedWeapon.Attack();
                lastAttackTime = Time.time; // Update the last attack time
                StartCoroutine(ResetSpriteAfterDelay(firingSpriteDuration)); // Reset sprite after specified duration
            }
            yield return null; // Wait until the next frame to continue the loop
        }

        playerSpriteRenderer.sprite = normalSprite; // Ensure sprite is reset when not attacking
    }

    private IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = normalSprite;
        }
    }

    public void setWeapon(RangedThing newWeapon) {
        rangedWeapon = newWeapon;
    }

    public void setSpriteRenderer(SpriteRenderer gun) {
        playerSpriteRenderer = gun;
    }

    public void setNormalSprite(Sprite sprite) {
        normalSprite = sprite;
    }

    public void setFiringSprite(Sprite sprite) {
        firingSprite = sprite;
    }
}
