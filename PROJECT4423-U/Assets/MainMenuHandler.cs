using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private ScreenFader screenFader;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
        //screenFader.FadeToColor("BasicMovement");
    }

    public void Quit()
    {
        Application.Quit();
    }
}