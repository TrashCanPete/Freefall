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



    //New Code Varibles

    public Transform rotator;

    public Vector3 inputValues;


    public float rotationSpeedXMin;
    public float rotationSpeedXMax;
    public float rotationSpeedX;
    public float rotationSpeedY;

    public float rotationSpeedZ;


    public float horizontalInput;
    public float verticalInput;

    public float smoothX;
    public float smoothY;
    public float smoothRotateDelayZ;

    public float rotateDelay;

    public float rotateXSmoothing;
    public float rotateYSmoothing;

    public float maxRotateX;
    public float minRotateX;

    public float rotateSpeedDelayX;
    public float rotateSpeedDelayY;

    public float rotateDelayZ;
    public float minRotateDelayZ;
    public float MaxRotateDelayZ;


    public float maxDelayTimeX;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;
        rb.useGravity = false;
        rb.isKinematic = true;

    }

    private void Update()
    {

        CamFollow();
    }
    private void FixedUpdate()
    {

        //Inputs
        horizontalInput = Input.GetAxis("Horizontal") * rotateYSmoothing;
        verticalInput = Input.GetAxis("Vertical") * rotateXSmoothing;

        //Rotation UP and DOWN Data
        //Glider rotation on the X axis and speeding up after a certian moment

        // after the vertical input has reached 1 or above
        if (verticalInput >= 1 || verticalInput <= -1)
        {
            //start rotation the glider on the x axis
            inputValues.x += rotationSpeedX * verticalInput * Time.deltaTime;
            inputValues.x = Mathf.Clamp(inputValues.x, maxRotateX, minRotateX);


            //delay to then rotate faster on the x axis
            //add 1 to the delay counter and clamping it to a certain value
            rotateSpeedDelayX += 1;
            rotateSpeedDelayX = Mathf.Clamp(rotateSpeedDelayX, 0, maxDelayTimeX);

            //once this value has reached the max value change the rotation speed to go faster
            if (rotateSpeedDelayX == maxDelayTimeX)
            {
                rotationSpeedX = Mathf.Lerp( rotationSpeedX, rotationSpeedXMax, smoothX);
            }
            else 
            {
                //Set the rotation speed back to the slower speed
                rotationSpeedX = rotationSpeedXMin;
            }

        }
        else 
        { 
            //reset the counter
            rotateSpeedDelayX = 0; 
        }

        // after the horizontal input has reached 1 or above
        if (horizontalInput >= 1 || horizontalInput <= -1)
        {
            inputValues.y += rotationSpeedY * horizontalInput * Time.deltaTime;


            //Z Tilt
            inputValues.z = -zRotation * Input.GetAxis("Horizontal") * rotationSpeedZ;
            inputValues.z = Mathf.Clamp(inputValues.z, -zRotation, zRotation);
        }
        else 
        {
            inputValues.z = Mathf.Clamp(inputValues.z, 0, rotationSpeedZ );
        }

        //Rotating LEFT and RIGHT Data
        //glider rotating on the y Axis
        inputValues.y += rotationSpeedY * Input.GetAxis("Horizontal") * Time.deltaTime;


        //function that actually performs the rotating
        transform.rotation = Quaternion.Euler(inputValues);



        //DEBUG DRAW LINES

        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + rb.velocity, Color.cyan);

        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + transform.forward * 10, Color.blue);
        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + transform.up * 5, Color.green);
        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + transform.right * 5, Color.red);

        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + Vector3.up * 15, Color.green);
        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + Vector3.forward * 15, Color.blue);
        Debug.DrawLine(playerPosition.transform.position, playerPosition.transform.position + Vector3.right * 15, Color.red);

        //OldCode();
    }
    public void CamFollow()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 0.5f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "UpDraft")
        {

            Debug.Log("In the updraft");
            transform.position += Vector3.up * 2;
        }
    }
    public void OldCode()
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



        rb.AddForce(transform.up * force * -yVelocity);


        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        //transform.up is the direction the force is being added
        //force is the amount of resistence added to the wingsuit
        //horizontal = more resistence
        //high angle, facing up or down = less resistence
        //yVelocity is the amount of forcing being added
    }
}
