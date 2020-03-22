using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GliderController : MonoBehaviour
{
    public GameObject gliderIn;
    public GameObject gliderOut;
    // Get the player Rigidbody component
    public Rigidbody rb;
    // Rotation
    [SerializeField]
    private Vector3 rot;

    //Camera follow distance
    [Header("Cam Variables")]

    [SerializeField]
    private float camFollowDist;
    [SerializeField]
    private float camHeightDist;


    [SerializeField]
    private float offSet;

    [SerializeField]
    public float maxFOV;
    [SerializeField]
    public float minFOV;
    [SerializeField]
    private float boostFOV;
    [SerializeField]
    private float standardMaxFOV;

    [SerializeField]
    private bool High;
    [SerializeField]
    private bool Mid;
    [SerializeField]
    private bool Low;
    [SerializeField]
    private bool isInTerminalVelocity;
    [SerializeField]
    private bool canBoost;

    [SerializeField]
    private bool Wingsin;

    //basic variable trackers------------------------------basic variable trackers------------------------------basic variable trackers------------------------------
    [Header("Basic Variables")]
    [SerializeField]
    private float yAngle;
    public float Speed;
    [SerializeField]
    private float currentTargetSpeed;
    [SerializeField]
    public float currentTargetForce;


    //gliders velocity variables----------------------------gliders velocity variables----------------------------gliders velocity variables----------------------------
    [Header("Velocity Variables")]
    private Vector3 baseVelocity;
    [SerializeField]
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

    //glider force variables---------------------------------glider force variables---------------------------------glider force variables---------------------------------
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

    //rotation variables--------------------------------------rotation variables--------------------------------------rotation variables--------------------------------------
    [Header("Rotation Variables")]
    [SerializeField]
    private float xRotationSpeed;
    [SerializeField]
    private float yRotationSpeed;
    [SerializeField]
    private float minXAngle;
    [SerializeField]
    private float maxXAngle;

    [Header("Yaw Speeds - Up and Down")]
    //Yaw-------------------------
    [SerializeField]
    private float currentYawRotationSpeed;
    [SerializeField]
    private float maxYawSpeedRotation;
    [SerializeField]
    private float minYawSpeedRotation;
    [SerializeField]
    private float YawrotationChangeRate;

    [Header("Pitch Speeds - Left and Right")]
    //Pitch--------------------
    [SerializeField]
    private float currentPitchRotationSpeed;
    [SerializeField]
    private float maxPitchSpeedRotation;
    [SerializeField]
    private float minPitchSpeedRotation;
    [SerializeField]
    private float PitchrotationChangeRate;


    [Header("Angle Variables")]
    [SerializeField]
    private float diveThreshold;
    [SerializeField]
    private float riseThreshold;

    //Counters----------------------------------------------//Counters----------------------------------------------//Counters----------------------------------------------
    [Header("Counters")]
    [SerializeField]
    private float maxDivingCounter;
    [SerializeField]
    private float divingCounterRate;
    [SerializeField]
    private float divingCounterStep;

    [SerializeField]
    private float maxRisingCounter;
    [SerializeField]
    private float risingCounterRate;
    [SerializeField]
    private float risingCounterStep;

    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float cameraSpeedOffset;

    //Boost-------------------------------------------------//Boost-------------------------------------------------//Boost-------------------------------------------------
    [SerializeField]
    private float currentBoostCounter;
    [SerializeField]
    private float maxBoostCounter;
    [SerializeField]
    private float boostCounterRate;
    [SerializeField]
    private float boostForce;
    [SerializeField]
    private float boostSpeed;



    private float colliderXIn = 0.3f;
    private float colliderXOut = 2.4f;
    private BoxCollider playerCollider;




    //UpDraft Variables------------------------------------UpDraft Variables------------------------------------UpDraft Variables------------------------------------
    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;

    private void Start()
    {
        playerCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;
        Wingsin = false;
        gliderIn.SetActive(false);
        gliderOut.SetActive(true);
    }

    private void Update()
    {
        DebugLines();
        CamFollow();

        //Getting the input data for rotatiing
        yaw = yRotationSpeed * Input.GetAxis("Horizontal") * currentYawRotationSpeed * Time.deltaTime;
        pitch = xRotationSpeed * Input.GetAxis("Vertical") * currentPitchRotationSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Wingsin = true;
            gliderIn.SetActive(true);
            gliderOut.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Wingsin = false;
            gliderIn.SetActive(false);
            gliderOut.SetActive(true);

        }
        
        PlayerRotationSpeeds();
        PlayerRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        cameraSpeed = Speed + cameraSpeedOffset;
        rot.x += 0.05f;
        FlyingStates();
        ReduceAddVelocity();
        UpDraftCounter();

        Speed = baseVelocity.magnitude;
        Speed = Mathf.Lerp(Speed, currentTargetSpeed, currentTargetForce * Time.deltaTime);
        baseVelocity = (rb.transform.forward * Speed);


        rb.velocity = baseVelocity + addedVelocity;


        TerminalBoost();


    }


    public void FlyingStates()
    {
        //Standard------------------------
        if (yAngle <= diveThreshold && yAngle >= riseThreshold)
        {
            High = false;
            Mid = true;
            Low = false;

            Debug.Log("Standard");
            ResetSpeedAndForceValues();

            risingCounterRate = 0;
            divingCounterRate = 0;


            if (divingCounterRate == 0)
            {
                ResetSpeedAndForceValues();
            }
        }
        //Diving------------------------------------------
        else if (yAngle >= diveThreshold)
        {
            High = false;
            Mid = false;
            Low = true;

            Debug.Log("Diving");
            currentTargetSpeed = divingMaxVelocity;
            currentTargetForce = divingForce;
            divingCounterRate += divingCounterStep;
            if (divingCounterRate >= maxDivingCounter)
            {
                Debug.Log("Terminal Velocity!!!!!!");
                currentTargetSpeed = terminalVelocity;
                currentTargetForce = terminalForce;
                isInTerminalVelocity = true;
                if (Wingsin == true)
                {
                    canBoost = true;
                }
            }
        }
        //Rising-----------------------------------
        else if (yAngle <= riseThreshold)
        {
            High = true;
            Mid = false;
            Low = false;

            Debug.Log("Rising");
            currentTargetSpeed = risingMaxVelocity;
            currentTargetForce = risingForce;
            risingCounterRate += risingCounterStep;

            if (risingCounterRate >= maxRisingCounter)
            {
                Debug.Log("Max Climb");
                currentTargetSpeed = maxRisingVelocity;
                currentTargetForce = maxRisingForce;
            }
        }
    }
    public void TerminalBoost()
    {
        if (yAngle <= diveThreshold)
        {
            isInTerminalVelocity = false;

            if (!canBoost)
            {
                Debug.Log("canBoost is false");
            }
            else if (canBoost == true && Wingsin == false)
            {
                canBoost = false;      
            }
        }

    }
    public void ResetSpeedAndForceValues()
    {
        currentTargetSpeed = standardMaxVelocity;
        currentTargetForce = standardForce;
    }
    public void ReduceAddVelocity()
    {
        //Reduce the added velocity to 0 over time
        addedVelocity = Vector3.Lerp(addedVelocity, Vector3.zero, addedForceReduction * Time.deltaTime);

    }

    //Player Rotation------------------------------------Player Rotation------------------------------------Player Rotation------------------------------------
    public void PlayerRotationSpeeds()
    {
        if (Speed <= 100)
        {
            currentPitchRotationSpeed = Mathf.Lerp(currentPitchRotationSpeed, maxPitchSpeedRotation, PitchrotationChangeRate);
            currentYawRotationSpeed = Mathf.Lerp(currentYawRotationSpeed, maxYawSpeedRotation, YawrotationChangeRate);
        }
        else if (Speed >= 100)
        {
            currentPitchRotationSpeed = Mathf.Lerp(currentPitchRotationSpeed, minPitchSpeedRotation, PitchrotationChangeRate);
            currentYawRotationSpeed = Mathf.Lerp(currentYawRotationSpeed, minYawSpeedRotation, YawrotationChangeRate);
        }
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



    //Interaction-------------------------------------------Interaction-------------------------------------------Interaction-------------------------------------------
    public void UpDraftCounter()
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
    }


    //Camera Functions-------------------------------------Camera Functions-------------------------------------Camera Functions-------------------------------------
    public void CamFollow()
    {
        //3.5 0.75 close 7 2 far

        Camera.main.fieldOfView = Mathf.Abs(Speed / offSet + cameraSpeedOffset);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * (camHeightDist);
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
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
