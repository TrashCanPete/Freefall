using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public AudioManager audioManager;

    private void Awake()
    {
        FindObjectOfType<AudioManager>().StopPlayingAudio("Boost");
    }
    private void Start()
    {
        
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
        SceneManager.LoadScene("Peter's Test Scene");
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
