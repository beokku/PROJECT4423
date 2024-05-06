using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUILevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Creature player;

    public void updateCounter() {
        level.text = player.getLevel().ToString();
    }
}
