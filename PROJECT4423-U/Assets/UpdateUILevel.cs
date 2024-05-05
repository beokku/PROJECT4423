using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUILevel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Creature player;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCounter()
    {
            level.text = player.getLevel().ToString();
    }
}
