using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
   
{
    public GameObject endScreenUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            Invoke("LoadScene", 2);
            endScreenUI.SetActive(true);
        }
    }  

    void LoadScene()
    {
        SceneManager.LoadScene("LevelBlockout_2");
        endScreenUI.SetActive(false);
    }
}

