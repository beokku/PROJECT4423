using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 0f;
    [SerializeField] float jumpForce = 3;
    [SerializeField] public int health = 10;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int stamina = 3;
    [SerializeField] float boostForce = 20;
    [SerializeField] int damage = 2;        // Damage Enemy Deals
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] bool dead = false;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1.0f;


    public enum CreatureMovementType { tf, physics };
    [SerializeField] CreatureMovementType movementType = CreatureMovementType.tf;
    public enum CreaturePerspective { topDown, sideScroll };
    [SerializeField] CreaturePerspective perspectiveType = CreaturePerspective.topDown;

    [Header("Physics")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpOffset = -.5f;
    [SerializeField] float jumpRadius = .25f;

    [Header("Flavor")]
    [SerializeField] string creatureName;
    public GameObject body;
    [SerializeField] private List<AnimationStateChanger> animationStateChangers;
    [SerializeField] public ParticleSystem deathParticles;
    [SerializeField] public ParticleSystem deathParticles2;
    [SerializeField] public ParticleSystem deathParticles3;
    [SerializeField] public ParticleSystem hitParticles;
    [SerializeField] private Color originalColor;

    [Header("Tracked Data")]
    [SerializeField] Vector3 homePosition = Vector3.zero;
    [SerializeField] CreatureSO creatureSO;
    //[SerializeField] bool isGrounded;
    [SerializeField] private float originalGravityScale;
    [SerializeField] int collisionCount;
    [SerializeField] public int enemiesDefeated = 0;
    [SerializeField] public int damageDealt = 0;

    Rigidbody2D rb;
    private float invulnerabilityTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originalColor = body.GetComponent<SpriteRenderer>().color;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

    }


    // Update is called once per frame
    void Update()
    {
        /*if ((Physics2D.OverlapCircleAll(transform.position + new Vector3(0, jumpOffset, 0), jumpRadius, groundMask).Length > 0))
        {
            isGrounded = true;
        } else { isGrounded = false; }
        */

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

        
            MoveCreatureTransform(direction);
        /*
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
        } */
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

   
    public void MoveCreatureTransform(Vector3 direction)
    {

        transform.position += direction * Time.deltaTime * speed;
    }

    public void OnAnimatorMove()
    {
  
    }
    /* public void Jump()
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
    }*/

    void takeDamage(int damageAmount)
    {
        if (!isInvulnerable)
        {
            CameraShake cameraShaker = Camera.main.GetComponent<CameraShake>();


            if (this.gameObject.tag == "Player")
            {
                
                if (cameraShaker != null)
                {
                    StartCoroutine(StartInvulnerability(0.5f));
                    StartCoroutine(cameraShaker.Shake(0.1f, 0.4f));
                    
                }
            }
            if (this.gameObject.tag == "Enemy")
            {
                
                if (cameraShaker != null)
                {
                    StartCoroutine(StartInvulnerability(0.05f));
                    StartCoroutine(cameraShaker.Shake(0.05f, 0.1f));
                }
            }
            health -= damageAmount;
            healthBar.UpdateHealthBar(health, maxHealth);

            if (health < (maxHealth * 0.3))
            {
                body.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            if (health < 1)
            {
                if (Random.Range(1, 5) == 1)
                {
                    this.gameObject.GetComponent<HealthSpawner>().spawnHealth();
                }

                if (this.gameObject.tag == "Enemy")
                {
                    GameObject playerObject = GameObject.FindWithTag("Player");
                    Creature playerCreature = playerObject.GetComponent<Creature>();
                    playerCreature.enemiesDefeated += 1;

                   
                    ImageShaker imageShaker = FindObjectOfType<ImageShaker>();
                    
                   
                    if (imageShaker != null)
                    {
                        StartCoroutine(imageShaker.Shake(0.1f, 20.0f));
                        
                    }

                    Destroy(this.gameObject);
                }

                if (this.gameObject.tag == "Player")
                {
                    dead = true;
                    //StartCoroutine(SlowTimeOnDeath());
                    DestroyAllEnemies();
                    DestroyAllSpawners();
                    this.gameObject.SetActive(false);
                    PauseMenuHandler pause = FindObjectOfType<PauseMenuHandler>();
                    pause.gameOver();
                }

                Instantiate(deathParticles, transform.position, Quaternion.identity);
                Instantiate(deathParticles2, transform.position, Quaternion.identity);
                Instantiate(deathParticles3, transform.position, Quaternion.identity);
                //Instantiate(hitParticles, transform.position, Quaternion.identity);

            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon" && this.gameObject.tag != "Player")
        {
            Thing attackerDamage = other.gameObject.GetComponent<Thing>();
            takeDamage(attackerDamage.damage);

            GameObject playerObject = GameObject.FindWithTag("Player");
            Creature playerCreature = playerObject.GetComponent<Creature>();
            playerCreature.damageDealt += attackerDamage.damage;
        }

        if (other.gameObject.tag == "Enemy" && this.gameObject.tag == "Player")
        {
            Creature enemyCreature = other.GetComponent<Creature>();
            int enemyDamage = enemyCreature.damage;
            takeDamage(enemyDamage);

        }

        if (other.gameObject.tag == "Item" && this.gameObject.tag == "Player")
        {
            health = Mathf.Min(health + 10, maxHealth);
            healthBar.UpdateHealthBar(health, maxHealth);

            if (health > (maxHealth * 0.3))
            {
                body.GetComponent<SpriteRenderer>().color = originalColor;
            }
            Destroy(other.gameObject);

        }
    }


    void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)        // Loop through the array and destroy each enemy
        {
            enemies[i].GetComponent<Creature>().takeDamage(99);
        }
    }

    void DestroyAllSpawners()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawners.Length; i++)       // Loop through the array and destroy each Spawner
        {
            Destroy(spawners[i]);
        }
    }

    IEnumerator StartInvulnerability(float length)
    {
        isInvulnerable = true;
        for (float i = 0; i < length; i += 0.2f) // Flashing interval
        {
            body.GetComponent<SpriteRenderer>().enabled = !body.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.2f);
        }
        // Ensure sprite is visible after flashing
        body.GetComponent<SpriteRenderer>().enabled = true;
        isInvulnerable = false;
    }

   



}