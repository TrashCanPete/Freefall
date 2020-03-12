using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WingsuitController : MonoBehaviour
{
    // Get the player Rigidbody component
    Rigidbody rb;
    // Rotation
    Vector3 rot;

    // Min angle for the player to rotate on the x-axis
    [SerializeField]
    private float minAngle = 0;
    // Max angle
    [SerializeField]
    private float maxAngle = 45;

    [SerializeField]
    private float maxTilt = 5;

    //Camera follow distance
    public float camFollowDist;

    // Audio mixer, to control the sound FX pitch
    public AudioMixer am;


    //gliders velocity value
    public float displayVelocity;
    public float displayAngle;

    public float yVelocity;
    public float force;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private float rotationSpeedX;
    [SerializeField]
    private float rotationSpeedY;
    [SerializeField]
    private float rotationSpeedZ;

    [SerializeField]
    private float minForce;
    [SerializeField]
    private float maxForce;
    [SerializeField]
    private float maxSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;

    }

    private void Update()
    {

        CamFollow();

        //Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Do any animation
    }

    private void FixedUpdate()
    {
        //glider rotating on the y Axis'}
        rot.x += rotationSpeedX * verticalInput * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, minAngle, maxAngle);
        rot.y += rotationSpeedY * horizontalInput * Time.deltaTime;
        rot.z = -rotationSpeedZ * Input.GetAxis("Horizontal");
        rot.z = Mathf.Clamp(rot.z, -maxTilt, maxTilt);

        //function that actually performs the rotating
        rb.MoveRotation(Quaternion.Euler(rot));

        // Get the percentage
        var percentage = rot.x / maxAngle;
        percentage = Mathf.Abs(percentage);

        yVelocity = rb.transform.InverseTransformDirection(rb.velocity).y;
        force = Mathf.Lerp(minForce, maxForce, percentage);

        rb.AddForce(rb.transform.up * force * -yVelocity);

        //DEBUG DRAW LINES

        DebugLines();

        //OldCode();
    }
    
    public float GetShake()
    {
        return 0;
    }

    public void CamFollow()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 0.5f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }
   /* private void OnTriggerStay(Collider other)
    {
        if (other.tag == "UpDraft")
        {

            Debug.Log("In the updraft");
            transform.position += Vector3.up * 2;
        }
    }
    */
    /*
    public void OldCode()
    {
        displayVelocity = rb.velocity.magnitude;
        //displayAngle = angle;



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
    */

    public void MoreRefinedRotation()
    {
        /*
         //X turning
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
*/

        /*
                 //Rotating LEFT and RIGHT Data
         //Y Turning
if (horizontalInput >= 1 || horizontalInput <= -1)
{
    inputValues.y += rotationSpeedY * horizontalInput * Time.deltaTime;

    rotateSpeedDelayY += 1;
    rotateSpeedDelayY = Mathf.Clamp(rotateSpeedDelayY, 0, maxDelayTimeY);
    if (rotateSpeedDelayY == maxDelayTimeY)
    {
        rotationSpeedY = Mathf.Lerp(rotationSpeedY, rotationSpeedYMax, smoothY);
    }
    else
    {
        rotationSpeedY = rotationSpeedYMin;
    }
}
else 
{
    rotateSpeedDelayY = 0;
}
*/

        /*
         // Z Turning
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
*/

    }

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
