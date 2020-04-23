using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OldWingSuit : MonoBehaviour
{
    // Get the player Rigidbody component
    public Rigidbody rb;
    // Rotation
    private Vector3 rot;

    public float MaxLowSpeed = 12.5f;

    public float lowSpeed;

    public float midSpeed;

    public float highSpeed;

    public float maxHighSpeed = 13.8f;

    // Max drag, if the player is on 0 deg or minAngle
    public float maxDrag = 6;
    // Min drag
    public float minDrag = 2;

    // Here we will store the modified force and drag
    private float mod_force;
    private float mod_drag;

    // Min angle for the player to rotate on the x-axis
    public float minAngle = 0;
    // Max angle
    public float maxAngle = 45;

    // Converting the x rotation from min angle to max, into a 0-1 format.
    // 0 means minAngle
    // 1 means maxAngle
    public float percentage;

    //Rotation Speeds
    public float xRotation;
    public float yRotation;
    public float zRotation;

    //Camera follow distance
    public float camFollowDist;

    // Audio mixer, to control the sound FX pitch
    public AudioMixer am;

    //Decrease Valuse
    public float decreaseNumber;

    //gliders velocity value
    public float Velocity;
    public float Force;

    public bool decreaseToggle = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;

    }

    private void Update()
    {

        Velocity = rb.velocity.magnitude;
        Force = mod_force;

        //rotate the player
        //X axis
        rot.x += xRotation * Input.GetAxis("Vertical") * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, minAngle, maxAngle);

        //Gradual decreasing in angle on the X axis
        rot.x -= +decreaseNumber * Time.deltaTime;
        //Y axis
        rot.y += yRotation * Input.GetAxis("Horizontal") * Time.deltaTime;
        //Clamped Z Axis rotation
        rot.z = -zRotation * Input.GetAxis("Horizontal");
        rot.z = Mathf.Clamp(rot.z, -zRotation, zRotation);
        transform.rotation = Quaternion.Euler(rot);

        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 0.50f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);

        // Speed and drag based on angle
        // Get the percentage (minAngle = 0, maxAngle = 1)
        percentage = rot.x / maxAngle;
        // Update parameters
        // If 0, we'll have maxDrag and lowSpeed
        // If 1, we'll get minDrag and highSpeed
        mod_drag = (percentage * (minDrag - maxDrag)) + maxDrag;

        mod_force = (percentage * (highSpeed - lowSpeed)) + lowSpeed;



        SpeedChanges();
        // Getting the local space of the velocity
        Vector3 localV = transform.InverseTransformDirection(rb.velocity);

        // Change z velocity to mod_force
        localV.z = mod_force;

        // Convert the local velocity back to world space and set it to the Rigidbody's velocity
        rb.velocity = transform.TransformDirection(localV);

        // Update drag to the modified one
        rb.drag = mod_drag;

        // Change pitch value based on the player's angle and percentage
        am.SetFloat("Pitch", 1 + percentage);



    }
    public void SpeedChanges()
    {

        //Percentage Values
        //-0.99 = straight up
        //-0.5  = 45 degrees up
        //0.0 = straight ahead
        //0.5 = 45 degrees down
        //0.9 = straight down

        if (!decreaseToggle)
        {
            return;
        }
        else 
        {
            if (Input.GetButton("Vertical"))
            {
                decreaseNumber = 0;
            }
            else
            {
                if (percentage >= 0.75f && percentage <= 1)
                {
                    decreaseNumber = 0;
                }
                else if (percentage >= 0.1f && percentage <= 0.90f)
                {
                    decreaseNumber = -5f;
                }
                else if (percentage <= 0.1 && percentage >= -0.1)
                {
                    decreaseNumber = -1;
                }
                else if (percentage <= -0.1 && percentage >= -0.5f)
                {
                    decreaseNumber = Mathf.Lerp(rot.x, -1, -10f);
                }
                else if (percentage <= -0.5f && percentage >= -1)
                {
                    decreaseNumber = Mathf.Lerp(rot.x, -1, -10f);
                }
            }
        }


        //mod_force = (percentage * (highSpeed - lowSpeed)) + lowSpeed;
    }
   
    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "UpDraft")
        {
            transform.position += Vector3.up *100;
        }
    }
    */
}
