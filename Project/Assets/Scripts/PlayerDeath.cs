using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void Death()
    {
        Invoke("ReloadCheckpoint", 1.5f);
    }

    void ReloadCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<AudioManager>().PlayAudio("Death");
    }
}
