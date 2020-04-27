using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public bool inMainMenu = true;

    public Animator animator;

    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject confirmationScreen;
    public GameObject backButton1;
    public GameObject backButton2;
    public GameObject backButton3;
    public GameObject backButton4;



    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        inMainMenu = false;
        mainMenuUI.SetActive(false);
        LoadGame();
    }

    void Update()
    {
        if (inMainMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }

                else
                {
                    Pause();
                }
            }       
        } 
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        confirmationScreen.SetActive(false);
        backButton1.SetActive(false);
        backButton2.SetActive(false);
        backButton3.SetActive(false);
        backButton4.SetActive(false);

    }

    void Pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0f;
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main_GameScene");
    }
    public void QuitToMenu()
    {
        inMainMenu = true;
        Resume();
        SceneManager.LoadScene("Main_Menu_2");
        mainMenuUI.SetActive(true);
    }
}
