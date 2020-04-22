using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;

    
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        LoadGame();
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        
        

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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
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
