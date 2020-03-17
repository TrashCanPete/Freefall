using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GliderController : MonoBehaviour
{
    // Get the player Rigidbody component
    public Rigidbody rb;
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
    public float Speed;
    [SerializeField]
    private float currentTargetSpeed;
    [SerializeField]
    public float currentTargetForce;

    //gliders velocity variables
    [Header("Velocity Variables")]
    private Vector3 baseVelocity;
    private Vector3 addedVelocity;

    [SerializeField]
    private float standardMaxVelocity;
    [SerializeField]
    private float divingMaxVelocity;
    [SerializeField]
    private float risingMaxVelocity;

    [SerializeField]
    private float terminalVelocity;
    [SerializeField]
    private float maxRisingVelocity;




    //glider force variables
    [Header("Force Variables")]


    [SerializeField]
    private float standardForce;
    [SerializeField]
    private float divingForce;
    [SerializeField]
    private float risingForce;
    [SerializeField]
    private float addedForceReduction;

    [SerializeField]
    private float terminalForce;
    [SerializeField]
    private float maxRisingForce;

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

    [SerializeField]
    private float maxSpeedRotation;
    [SerializeField]
    private float minSpeedRotation;

    [SerializeField]
    private float terminalRotation;
    [SerializeField]
    private float maxRisingRotation;


    [Header("Angle Variables")]
    [SerializeField]
    private float diveThreshold;
    [SerializeField]
    private float riseThreshold;

    [Header("Counters")]
    [SerializeField]
    private float maxDivingCounter;
    [SerializeField]
    private float divingCounter;
    [SerializeField]
    private float divingCounterStep;

    [SerializeField]
    private float maxRisingCounter;
    [SerializeField]
    private float risingCounter;
    [SerializeField]
    private float risingCounterStep;

    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;

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
            ResetSpeedAndForceValues();
            divingCounter = 0;
            risingCounter = 0;
            if (divingCounter == 0)
            {
                ResetSpeedAndForceValues();
            }
        }
        //Diving
        else if (yAngle >= diveThreshold)
        {
            Debug.Log("Diving");
            currentTargetSpeed = divingMaxVelocity;
            currentTargetForce = divingForce;
            divingCounter += divingCounterStep;
            if (divingCounter >= maxDivingCounter)
            {
                Debug.Log("Terminal Velocity!!!!!!");
                currentTargetSpeed = terminalVelocity;
                currentTargetForce = terminalForce;
            }
        }
        //Risinig
        else if (yAngle <= riseThreshold)
        {
            Debug.Log("Rising");
            currentTargetSpeed = risingMaxVelocity;
            currentTargetForce = risingForce;
            risingCounter += risingCounterStep;
            if (risingCounter >= maxRisingCounter)
            {
                Debug.Log("Max Climb");
                currentTargetSpeed = maxRisingVelocity;
                currentTargetForce = maxRisingForce;
            }
        }


        Speed = baseVelocity.magnitude;
        Speed = Mathf.Lerp(Speed, currentTargetSpeed, currentTargetForce * Time.deltaTime);
        baseVelocity = (rb.transform.forward * Speed);





        ReduceAddSpeed();
        UpDraftEvent();


        rb.velocity = baseVelocity + addedVelocity;
    }

    public void ResetSpeedAndForceValues()
    {
        currentTargetSpeed = standardMaxVelocity;
        currentTargetForce = standardForce;
    }
    public void ReduceAddSpeed()
    {
        //Reduce the added velocity to 0 over time
        addedVelocity = Vector3.Lerp(addedVelocity, Vector3.zero, addedForceReduction * Time.deltaTime);
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

    //Camera Functions-------------------------------------Camera Functions-------------------------------------Camera Functions-------------------------------------
    public void CamFollow()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * 0.50f;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }

    //Interaction-------------------------------------------Interaction-------------------------------------------Interaction-------------------------------------------
    public void UpDraftEvent()
    {
        var addedSpeed = addedVelocity.magnitude;
        if (addedSpeed > maxCurrentSpeedInUpDraft)
        {
            baseVelocity *= upDraftForwardVelocity;
        }
    }
    public void WindMovePlayer(Vector3 _windStrength)
    {
        Debug.Log("Pushed by wind");
        addedVelocity += _windStrength;
        Debug.Log(addedVelocity);
    }


    //Debugging----------------------------------------------Debugging----------------------------------------------Debugging----------------------------------------------
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
    public void TestingKeys()
    {
        //Test the vertical Up Draft
        if (Input.GetKey(KeyCode.Space))
        {
            WindMovePlayer(Vector3.up * 100);
        }
    }
}
