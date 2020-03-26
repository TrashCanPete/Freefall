using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLines : MonoBehaviour
{
    public GliderController gliderController;
    private FlyingStates flyingStates;
    // Start is called before the first frame update
    void Start()
    {
        flyingStates = GetComponent<FlyingStates>();
        gliderController.GetComponent<GliderController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugDrawLines()
    {
        Debug.DrawLine(transform.position, transform.position + flyingStates.rb.velocity, Color.cyan);

        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.up * 5, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.right * 5, Color.red);

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 15, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * 15, Color.blue);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * 15, Color.red);
    }
}
