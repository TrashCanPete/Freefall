using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WingsuitController : MonoBehaviour
{
    // Get the player Rigidbody component
    public Rigidbody rb;
    // Rotation
    public Vector3 rot;

    // Min angle for the player to rotate on the x-axis
    public float minAngle = 0;
    // Max angle
    public float maxAngle = 45;

    // Converting the x rotation from min angle to max, into a 0-1 format.
    // 0 means minAngle
    // 1 means maxAngle
    public float angle;

    //Rotation Speeds
    public float xRotation;
    public float yRotation;
    public float zRotation;

    //Camera follow distance
    public float camFollowDist;

    // Audio mixer, to control the sound FX pitch
    public AudioMixer am;


    //gliders velocity value
    public float displayVelocity;
    public float displayAngle;

    public float yVelocity;
    public float force;

    public Transform playerPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;

    }

    private void Update()
    {

        CamFollow();
    }
    private void FixedUpdate()
    {
        displayVelocity = rb.velocity.magnitude;
        displayAngle = angle;



        //rotate the player

        //X axis
        rot.x += xRotation * Input.GetAxis("Vertical") * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, minAngle, maxAngle);

        //Y axis
        rot.y += yRotation * Input.GetAxis("Horizontal") * Time.deltaTime;

        //Clamped Z Axis rotation
        rot.z = -zRotation * Input.GetAxis("Horizontal");
        rot.z = Mathf.Clamp(rot.z, -zRotation, zRotation);
        transform.rotation = Quaternion.Euler(rot);



        // Get the angle
        angle = rot.x / maxAngle;

        //making sure the percentage (the angle the wingsuit is facing) will always be positive

        //(faceing down = 1 facing straigh on = 0 facing up = 1)
        angle = Mathf.Abs(angle);

        float minForce = 0;
        float maxForce = 10;
        float maxSpeed = 100; //m/s

        //the the local velocity of the glider on the y axis 
        yVelocity = transform.InverseTransformDirection(rb.velocity).y;

        //Determining how much resistence to add depending of the angle(percentage)
        force = Mathf.Lerp(maxForce, minForce, angle);

        Debug.DrawLine(playerPosition.transform.position, transform.forward, Color.red);

        rb.AddForce(transform.up * force*-yVelocity);
        Debug.DrawLine(playerPosition.transform.position, transform.up, Color.green);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        Debug.DrawLine(playerPosition.transform.position, rb.velocity, Color.blue);
        //transform.up ios the direction the force is being added
        //force is the amount of resistence added to the wingsuit
        //horizontal = more resistence
        //high angle, facing up or down = less resistence
        //yVelocity is the amount of forcing being added
    }
    public void CamFollow()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 2.0f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }
}
