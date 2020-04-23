using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPosition : MonoBehaviour
{
    private GameMaster gm;
    private PlayerDeath playerDeath;
    private Animator anim;
    
    private void Start()
    {
        playerDeath = GetComponent<PlayerDeath>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        anim = GetComponentInChildren<Animator>();
        transform.position = gm.lastCheckpointPos;
    }


  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            anim.SetTrigger("Dead");
            playerDeath.Death();
            
        }
    }
}
