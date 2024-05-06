using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour {

    [SerializeField] GameObject UI;
    [SerializeField] TextMeshProUGUI text;
 
    void Update() {
         if (Input.GetKeyDown(KeyCode.Escape)) {
        if (UI.activeSelf)
            Resume(); 
        else
            Pause(); 
        }
    }

    public void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void gameOver() {
        UI.SetActive(true);
        text.text = "Game Over";
    }

    public void Quit() {
        Application.Quit();
    }

    public void Pause() {
        text.text = "Game Paused";
        Time.timeScale = 0;
        UI.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1;
        UI.SetActive(false);
    }
}