using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDefeated : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Creature player;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCounter();
    }

    void updateCounter()
    {
            counter.text = player.enemiesDefeated.ToString();
    }
}
