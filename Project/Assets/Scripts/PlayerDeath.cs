using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        
    }
    public void Death()
    {
        Invoke("ReloadCheckpoint", 2f);
        anim.SetTrigger("Fade_Out");
    }

    void ReloadCheckpoint()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<AudioManager>().PlayAudio("Death");
    }
}
