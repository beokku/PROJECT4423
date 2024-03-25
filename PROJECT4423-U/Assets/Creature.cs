using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 0f;
    [SerializeField] float jumpForce = 3;
    [SerializeField] int health = 10;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int stamina = 3;
    [SerializeField] float boostForce = 20;
    [SerializeField] int damage = 2;        // Damage Enemy Deals
    [SerializeField] FloatingHealthBar healthBar;

    public enum CreatureMovementType { tf, physics };
    [SerializeField] CreatureMovementType movementType = CreatureMovementType.tf;
    public enum CreaturePerspective { topDown, sideScroll };
    [SerializeField] CreaturePerspective perspectiveType = CreaturePerspective.topDown;

    [Header("Physics")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpOffset = -.5f;
    [SerializeField] float jumpRadius = .25f;

    [Header("Flavor")]
    [SerializeField] string creatureName = "Meepis";
    public GameObject body;
    [SerializeField] private List<AnimationStateChanger> animationStateChangers;
    [SerializeField] public ParticleSystem deathParticles;
    [SerializeField] public ParticleSystem deathParticles2;

    [Header("Tracked Data")]
    [SerializeField] Vector3 homePosition = Vector3.zero;
    [SerializeField] CreatureSO creatureSO;
    [SerializeField] bool isGrounded;
    [SerializeField] private float originalGravityScale;
    [SerializeField] int collisionCount;
  

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        Debug.Log(health);

    }



    // Update is called once per frame
    void Update()
    {
        if ((Physics2D.OverlapCircleAll(transform.position + new Vector3(0, jumpOffset, 0), jumpRadius, groundMask).Length > 0))
        {
            isGrounded = true;
        } else { isGrounded = false; }


        if (creatureSO != null)
        {
            creatureSO.health = health;
            creatureSO.stamina = stamina;
        }

       

    }

    void FixedUpdate()
    {
        collisionCount = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, jumpOffset, 0), jumpRadius, groundMask).Length;
    }

        public void MoveCreature(Vector3 direction)
    {

        //if (movementType == CreatureMovementType.tf)
       // {
            MoveCreatureTransform(direction);
      //  }
        /*else if (movementType == CreatureMovementType.physics)
        {
            MoveCreatureRb(direction);
        }*/

        //set animation
        if (direction != Vector3.zero)
        {
            foreach (AnimationStateChanger asc in animationStateChangers)
            {
                asc.ChangeAnimationState("Walk", speed);
            }
        }
        else
        {
            foreach (AnimationStateChanger asc in animationStateChangers)
            {
                asc.ChangeAnimationState("Idle");
            }
        }
    }

    public void MoveCreatureToward(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        // MoveCreature(direction.normalized);
        MoveCreatureTransform(direction);
    }

    public void Stop()
    {
        MoveCreature(Vector3.zero);
    }

    public void MoveCreatureRb(Vector3 direction)
    {
        Vector3 currentVelocity = Vector3.zero;
        if (perspectiveType == CreaturePerspective.sideScroll)
        {
            currentVelocity = new Vector3(0, rb.velocity.y, 0);
            direction.y = 0;
        }

        rb.velocity = (currentVelocity) + (direction * speed);
        if (rb.velocity.x < 0)
        {
            body.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.velocity.x > 0)
        {
            body.transform.localScale = new Vector3(1, 1, 1);
        }
        //rb.AddForce(direction * speed);
        //rb.MovePosition(transform.position + (direction * speed * Time.deltaTime))
    }

    public void MoveCreatureTransform(Vector3 direction)
    {

        transform.position += direction * Time.deltaTime * speed;
    }

    public void OnAnimatorMove()
    {
  
    }
    public void Jump()
    {
        if (Physics2D.OverlapCircleAll(transform.position + new Vector3(0, jumpOffset, 0), jumpRadius, groundMask).Length > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Boost() {
        // Determine the horizontal direction based on the current velocity
        float horizontalDirection = Mathf.Sign(rb.velocity.x); // Will be -1 for left, 1 for right, and 0 if stationary

        // Ensure there's a horizontal direction to boost towards
        if (horizontalDirection != 0)
        {
            Vector2 boostDirection = new Vector2(horizontalDirection, 0).normalized;

            // Apply the boost force
            rb.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);
        }
    }

    void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health < (maxHealth * 0.3)) {
            body.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (health < 1)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Instantiate(deathParticles2, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon" && this.gameObject.tag != "Player")
        {
            Thing attackerDamage = other.gameObject.GetComponent<Thing>();
            takeDamage(attackerDamage.damage);
            
        }

        if (other.gameObject.tag == "Enemy" && this.gameObject.tag == "Player")
        {
            Creature enemyCreature = other.GetComponent<Creature>();
            int enemyDamage = enemyCreature.damage;
            takeDamage(enemyDamage);

        }


    }



}