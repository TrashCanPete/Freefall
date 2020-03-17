using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public Vector3 facingDirection;

    [SerializeField]
    private float WindStrength;

    private void Start()
    {
        facingDirection = transform.up;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player"))
        {
            other.GetComponent<GliderController>().WindMovePlayer(facingDirection * WindStrength);
            Debug.Log("Wind Pushing");
        }
    }


}

