using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDefeated : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Creature player;

    void Update() {
        updateCounter();
    }
    void updateCounter() {
            counter.text = player.enemiesDefeated.ToString();
    }
}
