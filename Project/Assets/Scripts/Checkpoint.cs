using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Checkpoint");
        if (other.tag == "Player")
        {
            gm.lastCheckpointPos = transform.position;
        }
    }
}
