using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour  
{

    public GameObject endScreenUI;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            endScreenUI.SetActive(true);
        }
    }
    public void StartWaitToEnd()
    {
        StartCoroutine(WaitToEnd());
    }
    public IEnumerator WaitToEnd()
    {
        endScreenUI.SetActive(true);
        Debug.Log("WaitToEnd");
        yield return new WaitForSeconds(3);
        LoadScene(0);
    }
    public void LoadScene(int _level)
    {
        endScreenUI.SetActive(false);
        SceneManager.LoadScene(_level);
    }
    public void ExitScene()
    {
        Application.Quit();
    }
}

