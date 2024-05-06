using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timerText;
    float elapsedTime;
    public GameObject player;
    public Creature playerCreature;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        playerCreature = player.GetComponent<Creature>();
    }
    void Update() {

        if (player == null || !player.gameObject.activeInHierarchy){

        } else {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}