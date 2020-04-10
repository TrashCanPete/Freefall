using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public Vector3 facingDirection;

    [SerializeField]
    private float windStrength;

    private void Start()
    {
        facingDirection = transform.up;
    }

    private void Update()
    {
        DebugLinesUpDraft();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player"))
        {
            other.GetComponent<GliderController>().WindMovePlayer(facingDirection * windStrength);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Player"))
        {
        }
    }

    public void DebugLinesUpDraft()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 30, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.up * 30, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.right * 30, Color.red);
    }

}

