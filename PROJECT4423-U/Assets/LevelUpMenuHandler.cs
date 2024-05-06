using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class LevelUpMenuHandler : MonoBehaviour {

    [SerializeField] GameObject UI;
    [SerializeField] private Creature playerCreature;
    [SerializeField] private GameObject chainGun;
    [SerializeField] private SpriteRenderer chainGunSpriteRenderer;
    [SerializeField] private Sprite chainNormalSprite;
    [SerializeField] private Sprite chainFiringSprite;
    [SerializeField] private GameObject carrotGun;
    [SerializeField] public GameObject upgradeAttackSpeedPrefab;
    [SerializeField] private GameObject upgradeProjectilesPrefab;
    [SerializeField] private GameObject upgradeExpMultPrefab;
    [SerializeField] private GameObject gameMusic;
    
    public bool canClick = true; 



    private CurWeapon currentWeapon;
    void Start() {
    if (playerCreature != null)
    {
        currentWeapon = playerCreature.getCurrentWeapon();
        if (currentWeapon == null)
        {
            Debug.LogError("CurWeapon component not found on playerCreature!");

        }
    } else {
        Debug.LogError("playerCreature not set!");
    }
    if (playerCreature.getLevel() == 1) {
        GetComponent<AudioSource>().Play();
        Time.timeScale = 0.0f;
        UI.transform.Find("LevelUpTitle").GetComponent<TextMeshProUGUI>().SetText("Welcome");
        UI.transform.Find("LevelUpText").GetComponent<TextMeshProUGUI>().SetText("Choose a Weapon");
    }
    
}


    public void show() {
        UI.SetActive(true);
        gameMusic.GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();

        if (playerCreature.getLevel() == 2) {
            UI.transform.Find("LevelUpTitle").GetComponent<TextMeshProUGUI>().SetText("Level Up");
            UI.transform.Find("LevelUpText").GetComponent<TextMeshProUGUI>().SetText("Choose an Upgrade");
            GameObject carrot = GameObject.Find("Upgrade_CarrotGun");
            GameObject chain = GameObject.Find("Upgrade_ChainGun");
            GameObject empty = GameObject.Find("Upgrade_Empty");
            if(carrot == null) {
                Debug.LogError("Upgrade_CarrotGun not found or already destroyed");
            }
            if(chain == null) {
                Debug.LogError("upgradeAttackSpeedPrefab is null in the inspector");
            }
            ReplacePrefab(carrot, upgradeAttackSpeedPrefab);
            ReplacePrefab(chain, upgradeProjectilesPrefab);
            ReplacePrefab(empty, upgradeExpMultPrefab);

        }

        StartCoroutine(DisableClicksCoroutine()); // Disable clicking for 1 second
    }

   public void upgradeAttackSpeed(float upgradeAmount) {
    if (!canClick) return; 
    currentWeapon.GetComponent<RangedThing>().setAttackSpeed(upgradeAmount);
    GetComponent<AudioSource>().Stop();
    UI.SetActive(false);
    Time.timeScale = 1;
    gameMusic.GetComponent<AudioSource>().Play();
}

    public void upgradeNumProjectiles(int num) {
        if (!canClick) return; 
        currentWeapon.GetComponent<RangedThing>().increaseProjectiles(num);
        currentWeapon.GetComponent<RangedThing>().setAttackSpeed(-0.20f);
        GetComponent<AudioSource>().Stop();
        UI.SetActive(false);
        Time.timeScale = 1;
        gameMusic.GetComponent<AudioSource>().Play();
    }

    public void upgradeExpMult(float amount) {
        if (!canClick) return; 
        playerCreature.addExpMult(amount);
        GetComponent<AudioSource>().Stop();
        UI.SetActive(false);
        Time.timeScale = 1;
        gameMusic.GetComponent<AudioSource>().Play();
    }

    public void chooseWeapon_ChainGun() {
        
        if (chainGun == null) {
            Debug.LogError("ChainGun not found in the scene.");
            return;
        }
        chainGun.SetActive(true);

        CurWeapon curWeapon = chainGun.GetComponent<CurWeapon>();
        if (curWeapon == null) {
            Debug.LogError("CurWeapon component not found on ChainGun.");
            return;
        }
        RangedThing rangedThing = chainGun.GetComponent<RangedThing>();
        if (rangedThing == null) {
            Debug.LogError("RangedThing component not found on ChainGun.");
            return;
        }
        curWeapon.setWeapon(rangedThing);

        if (playerCreature == null) {
            Debug.LogError("playerCreature is not assigned.");
            return;
        }
        playerCreature.setWeapon(curWeapon);

        GameObject inputHandler = GameObject.Find("PlayerInputHandler");
        if (inputHandler == null) {
            Debug.LogError("PlayerInputHandler not found in the scene.");
            return;
        }
        PlayerInputHandler playerInputHandler = inputHandler.GetComponent<PlayerInputHandler>();
        if (playerInputHandler == null) {
            Debug.LogError("PlayerInputHandler component not found on PlayerInputHandler GameObject.");
            return;
        }
        playerInputHandler.setWeapon(rangedThing);
        playerInputHandler.setSpriteRenderer(chainGunSpriteRenderer);
        playerInputHandler.setNormalSprite(chainNormalSprite);
        playerInputHandler.setFiringSprite(chainFiringSprite);
        currentWeapon = curWeapon;
        carrotGun.SetActive(false);
        chainGun.SetActive(true);
        GetComponent<AudioSource>().Stop();
        UI.SetActive(false);
        Time.timeScale = 1;
        gameMusic.GetComponent<AudioSource>().Play();
    }


    public void chooseWeapon_CarrotGun() {

        GetComponent<AudioSource>().Stop();
        // Disable Upgrade Screen
        UI.SetActive(false);
        // Timescale back to normal
        Time.timeScale = 1;
        gameMusic.GetComponent<AudioSource>().Play();

    }

    private IEnumerator DisableClicksCoroutine()
    {
        Debug.Log("Clicks disabled");
        canClick = false; 
        yield return new WaitForSecondsRealtime(1.0f);  // Wait for 1 second, unaffected by Time.timeScale
        canClick = true; 
        Debug.Log("Clicks enabled");
    }

    public void ReplacePrefab(GameObject currentPrefab, GameObject newPrefab)
    {
        if (currentPrefab != null && newPrefab != null)
        {
            
            Vector3 position = currentPrefab.transform.position;        // Position/Rotation of old prefab
            Quaternion rotation = currentPrefab.transform.rotation;
            Transform parent = currentPrefab.transform.parent;          // Grab parent for instantiation
            Destroy(currentPrefab);

        
            GameObject newInstance = Instantiate(newPrefab, position, rotation, parent);    // Instantiate the new prefab at the old prefab's position/rotation
        } else
        {
            Debug.LogError("Current prefab instance or new prefab is not set.");
        }
    }
}