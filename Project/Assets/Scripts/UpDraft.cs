using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public GliderController _gliderController;

    public Vector3 facingDirection;

    private void Start()
    {
        facingDirection = transform.up;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            other.GetComponent<GliderController>().WindMovePlayer(facingDirection * 100);
            Debug.Log("Wind Pushing");
        }
    }


}

