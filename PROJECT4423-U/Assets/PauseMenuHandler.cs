using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{

    [SerializeField] GameObject UI;
    // Start is called before the first frame update
    //[SerializeField] private ScreenFader screenFader;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //screenFader.FadeToColor("BasicMovement");
    }

    public void gameOver()
    {
        UI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}