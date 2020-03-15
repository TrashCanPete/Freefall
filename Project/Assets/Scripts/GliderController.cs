using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GliderController : MonoBehaviour
{
    // Get the player Rigidbody component
    private Rigidbody rb;
    // Rotation
    [SerializeField]
    private Vector3 rot;

    //Camera follow distance
    [Space(10)]
    [SerializeField]
    private float camFollowDist;

    //basic variable trackers
    [Header("Basic Variables")]
    [SerializeField]
    private float yAngle;
    public float Velocity;
    [SerializeField]
    private float currentTargetVelocity;
    [SerializeField]
    public float currentTargetForce;

    //gliders velocity variables
    [Header("Velocity Variables")]


    [SerializeField]
    private float standardMaxVelocity;
    [SerializeField]
    private float divingMaxVelocity;
    [SerializeField]
    private float risingMaxVelocity;

    //glider force variables
    [Header("Force Variables")]


    [SerializeField]
    private float standardForce;
    [SerializeField]
    private float divingForce;
    [SerializeField]
    private float risingForce;  

    //input variables
    private float pitch;
    private float yaw;

    //rotation variables
    [Header("Rotation Variables")]
    [SerializeField]
    private float xRotationSpeed;
    [SerializeField]
    private float yRotationSpeed;
    [SerializeField]
    private float minXAngle;
    [SerializeField]
    private float maxXAngle;

    [Header("Angle Variables")]
    [SerializeField]
    private float diveThreshold;
    [SerializeField]
    private float riseThreshold;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;

    }

    private void Update()
    {
        DebugLines();
        CamFollow();

        //Getting the input data for rotatiing
        yaw = yRotationSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        pitch = xRotationSpeed * Input.GetAxis("Vertical") * Time.deltaTime;

        PlayerRotation();

    }

    private void FixedUpdate()
    {
        if (yAngle <= diveThreshold && yAngle >= riseThreshold)
        {
            Debug.Log("Standard");
            currentTargetVelocity = standardMaxVelocity;
            currentTargetForce = standardForce;
        }
        //Diving
        else if (yAngle >= diveThreshold)
        {
            Debug.Log("Diving");
            currentTargetVelocity = divingMaxVelocity;
            currentTargetForce = divingForce;

        }
        //Risinig
        else if (yAngle <= riseThreshold)
        {
            Debug.Log("Rising");
            currentTargetVelocity = risingMaxVelocity;
            currentTargetForce = risingForce;
        }

        Velocity = rb.velocity.magnitude;
        Velocity = Mathf.Lerp(Velocity, currentTargetVelocity, currentTargetForce * Time.deltaTime);
        rb.velocity = (transform.forward * Velocity);
    }

    public void CamFollow()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 0.50f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }

    public void PlayerRotation()
    {
        //Getting the angle that the glider is facing
        yAngle = rot.x;

        //adding the pitch inputs/data into the rot.x plus clamping the values
        rot.x += pitch;
        //max being the being first as when looking up the rotation is at its lowest, vise versa
        rot.x = Mathf.Clamp(rot.x, maxXAngle, minXAngle);


        rot.y += yaw;

        //rotating the glider by the rigidbody via the rot values
        rb.transform.rotation = Quaternion.Euler(rot);
    }


    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "UpDraft")
        {
            transform.position += Vector3.up *100;
        }
    }
    */
    private void DebugLines()
    {
        Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.cyan);

        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.up * 5, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.right * 5, Color.red);

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 15, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * 15, Color.blue);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * 15, Color.red);
    }
}
