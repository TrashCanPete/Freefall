using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public Animator animator;

    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject confirmationScreen;


    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        LoadGame();
    }

    void Update()
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

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        confirmationScreen.SetActive(false);
    }

    void Pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0f;
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
        SceneManager.LoadScene("Main_Menu_2");
    }
}
