using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Creature : MonoBehaviour {
    [Header("Stats")]
    public float speed = 0f;
    [SerializeField] float jumpForce = 3;
    [SerializeField] public int health = 10;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int level = 1;
    [SerializeField] int exp = 0;
    [SerializeField] float expMult = 1;
    [SerializeField] int maxExp = 10;
    [SerializeField] float expIncreasePercentage = 10f;
    [SerializeField] int levelUpMaxHealthIncrease = 1;
    [SerializeField] int stamina = 3;
    [SerializeField] float boostForce = 20;
    [SerializeField] int damage = 2;        // Damage Enemy Deals
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] public Slider expBar;
    [SerializeField] bool dead = false;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1.0f;

    [SerializeField] CurWeapon currentWeapon;


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
    private int oldEnemiesDefeated = 0;
    [SerializeField] public int damageDealt = 0;
    [SerializeField] private AudioSource playerDamage;
    [SerializeField] private AudioSource enemyDeath;
    [SerializeField] private AudioSource expPickup;
    [SerializeField] private AudioSource healthPickup;

    Rigidbody2D rb;
    private float invulnerabilityTimer;
    private UIExpBar CreatureExpBar;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start() {
        originalColor = body.GetComponent<SpriteRenderer>().color;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (this.gameObject.tag == "Player") {
            CreatureExpBar = expBar.GetComponent<UIExpBar>();
            CreatureExpBar.UpdateExpBar(exp, maxExp);
        }
    }

    void Update() {
        if (creatureSO != null) {
            creatureSO.health = health;
            creatureSO.stamina = stamina;
        }

        if (oldEnemiesDefeated < enemiesDefeated) {
            enemyDeath.Play();
            oldEnemiesDefeated = enemiesDefeated;
        }
    }

    void FixedUpdate() {
        collisionCount = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, jumpOffset, 0), jumpRadius, groundMask).Length;
    }

    public void MoveCreature(Vector3 direction) {
            MoveCreatureTransform(direction);
    }

    public void MoveCreatureToward(Vector3 target) {
        Vector3 direction = target - transform.position;
        // MoveCreature(direction.normalized);
        MoveCreatureTransform(direction);
    }

    public void Stop() {
        MoveCreature(Vector3.zero);
    }

    public void MoveCreatureTransform(Vector3 direction) {
        transform.position += direction * Time.deltaTime * speed;
    }

    void levelUp() {

        exp = 0;
        level += 1;
        float increaseAmount = maxExp * (expIncreasePercentage / 100f);
        maxExp = maxExp + (int)Math.Ceiling(increaseAmount);
        maxHealth += levelUpMaxHealthIncrease;
        Time.timeScale = 0;
        LevelUpMenuHandler levelUpMenu = FindObjectOfType<LevelUpMenuHandler>();
        levelUpMenu.show();

        GameObject levelText = GameObject.Find("/UI Canvas/LevelText");
        levelText.GetComponent<UpdateUILevel>().updateCounter();
    }

    void takeDamage(int damageAmount) {
        if (!isInvulnerable)
        {
            CameraShake cameraShaker = Camera.main.GetComponent<CameraShake>();
            if (this.gameObject.tag == "Player") {
                if (cameraShaker != null) {
                    StartCoroutine(StartInvulnerability(0.5f));
                    StartCoroutine(cameraShaker.Shake(0.1f, 0.4f)); 
                }
            }
            if (this.gameObject.tag == "Enemy") {
                GetComponent<AudioSource>().Play();
                if (cameraShaker != null) {
                    //StartCoroutine(StartInvulnerability(0.05f));
                    StartCoroutine(cameraShaker.Shake(0.01f, 0.05f));
                }
            }
            health -= damageAmount;
            healthBar.UpdateHealthBar(health, maxHealth);

            if (health < (maxHealth * 0.3) && this.gameObject.tag == "Player") {
                body.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            if (health < 1) {
                if (this.gameObject.tag == "Enemy") {
                    if (UnityEngine.Random.Range(1, 10) == 1) {
                        this.gameObject.GetComponent<HealthSpawner>().spawnHealth();
                    }
                    if (UnityEngine.Random.Range(1, 2) == 1) {
                        this.gameObject.GetComponent<ExpSpawner>().spawnExp();
                    }

                    GameObject playerObject = GameObject.FindWithTag("Player");
                    Creature playerCreature = playerObject.GetComponent<Creature>();
                    playerCreature.enemiesDefeated += 1;

                    ImageShaker imageShaker = FindObjectOfType<ImageShaker>();
                    
                    if (imageShaker != null) {
                        StartCoroutine(imageShaker.Shake(0.1f, 20.0f)); 
                    }

                   EnemyPoolManager.Instance.AddToPool(this.gameObject);
                   this.gameObject.SetActive(false);
                   health = maxHealth;
                   resetHealthBar();
                }

                if (this.gameObject.tag == "Player") {
                    dead = true;
                    DestroyAllEnemies();
                    DestroyAllSpawners();
                    this.gameObject.SetActive(false);
                    PauseMenuHandler pause = FindObjectOfType<PauseMenuHandler>();
                    pause.gameOver();
                }

                Instantiate(deathParticles, transform.position, Quaternion.identity);
                Instantiate(deathParticles2, transform.position, Quaternion.identity);
                Instantiate(deathParticles3, transform.position, Quaternion.identity);          
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
 
        if (other.gameObject.tag == "Enemy" && this.gameObject.tag == "Player") {
            Creature enemyCreature = other.GetComponent<Creature>();
            int enemyDamage = enemyCreature.damage;
            takeDamage(enemyDamage);
            playerDamage.Play();

        }

        // HEALTH PICKUP
        if (other.gameObject.tag == "HealthItem" && this.gameObject.tag == "Player") {
            health = Mathf.Min(health + 10, maxHealth);
            healthBar.UpdateHealthBar(health, maxHealth);

            if (health > (maxHealth * 0.3)) {
                body.GetComponent<SpriteRenderer>().color = originalColor;
            }
            Destroy(other.gameObject);
            healthPickup.Play();
        }

        // EXP PICKUP
        if (other.gameObject.tag == "ExpItem" && this.gameObject.tag == "Player") {
            int baseExp = 1; // Base experience from picking up an exp item
            int totalExp = (int)(baseExp * expMult); // Adjust base exp by the multiplier
            exp += totalExp; // Add to the total experience
        
            if (exp >= maxExp) {
                levelUp();
            }
            CreatureExpBar.UpdateExpBar(exp, maxExp);
            expPickup.Play();
            Destroy(other.gameObject); 
        }

        if (other.gameObject.tag == "PlayerProjectile" && this.gameObject.tag == "Enemy") {
            Projectile projectile = other.GetComponent<Projectile>();
            int damageToTake = projectile.damage;
            takeDamage(damageToTake);
        }
    }


    void DestroyAllEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++) {              // Loop through the array and destroy each enemy
            enemies[i].GetComponent<Creature>().takeDamage(99);
        }
    }

    void DestroyAllSpawners() {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawners.Length; i++) {             // Loop through the array and destroy each Spawner
            Destroy(spawners[i]);
        }
    }

    IEnumerator StartInvulnerability(float length) {
        isInvulnerable = true;
        for (float i = 0; i < length; i += 0.2f) {
            body.GetComponent<SpriteRenderer>().enabled = !body.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.2f);
        }
        body.GetComponent<SpriteRenderer>().enabled = true;
        isInvulnerable = false;
    }

    public CurWeapon getCurrentWeapon() { return currentWeapon;}
    public void setWeapon(CurWeapon weapon) { currentWeapon = weapon;}

   public int getLevel() { return level;}

   public void addExpMult(float amt) {expMult += amt;}

   public void resetHealth() { health = maxHealth;}

    public void resetHealthBar() {healthBar.UpdateHealthBar(maxHealth, maxHealth);}

}