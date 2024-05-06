using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupButtonActions : MonoBehaviour {

    public LevelUpMenuHandler lvlHandler;
    void Awake() {
        
        var button = GetComponent<Button>();
        if (button != null) {
            button.onClick.AddListener(ButtonClicked);
        }
    }

    void Start() {
        lvlHandler = FindObjectOfType<LevelUpMenuHandler>();
    }

    private void ButtonClicked() {
        String upgradeName = gameObject.name;

        switch (upgradeName) {
            case "Upgrade_AttackSpeed(Clone)":
                lvlHandler.upgradeAttackSpeed(0.10f);
                break;

            case "Upgrade_NumProjectiles(Clone)":
                lvlHandler.upgradeNumProjectiles(1);
                break;

            case "Upgrade_ExpMult(Clone)":
                lvlHandler.upgradeExpMult(0.10f);
                break;

            default:
                break;
            
        }
        Debug.Log("Button on instantiated prefab was clicked!");
    }
}
