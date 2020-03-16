using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public float WindStrength;
    public GliderController _gliderController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            _gliderController.WindMovePlayer(100);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player"))
        {
            
        }
    }
}

