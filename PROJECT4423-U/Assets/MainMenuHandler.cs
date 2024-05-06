using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {

    public void Play() {
        SceneManager.LoadScene("Level1");
    }

    public void Quit() {
        Application.Quit();
    }
}