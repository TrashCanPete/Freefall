using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GliderController : MonoBehaviour
{
    public GameObject boostLight;
    public GameObject meshGrp;

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
    private float currentYawRotationSpeed;
    [SerializeField]
    private float maxYawSpeedRotation;
    [SerializeField]
    private float minYawSpeedRotation;
    [SerializeField]
    private float YawrotationChangeRate;

    [Header("Pitch Speeds - Left and Right")]
    //Pitch--------------------
    private float currentPitchRotationSpeed;
    [SerializeField]
    private float maxPitchSpeedRotation;
    [SerializeField]
    private float minPitchSpeedRotation;
    [SerializeField]
    private float PitchrotationChangeRate;


    //Counters----------------------------------------------//Counters----------------------------------------------//Counters----------------------------------------------
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float cameraSpeedOffset;


    private float colliderXIn = 0.3f;
    private float colliderXOut = 2.4f;
    private BoxCollider playerCollider;


    //UpDraft Variables------------------------------------UpDraft Variables------------------------------------UpDraft Variables------------------------------------
    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;

    //public scripts
    public DebugLines debugLines;
    public FlyingStates flyingStates;

    Quaternion originalRotation;


    //Start
    private void Start()
    {
        originalRotation = meshGrp.transform.rotation;
        //Calling Scripts
        debugLines = GetComponent<DebugLines>();
        flyingStates = GetComponent<FlyingStates>();

        boostLight.SetActive(false);
        playerCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //drawing lines
        debugLines.DebugDrawLines();

        CamFollow();

        //Getting the input data for rotatiing
        yaw = yRotationSpeed * Input.GetAxis("Horizontal") * currentYawRotationSpeed * Time.deltaTime;
        pitch = xRotationSpeed * Input.GetAxis("Vertical") * currentPitchRotationSpeed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            flyingStates.isBoosting = true;
            boostLight.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            flyingStates.isBoosting = false;
            boostLight.SetActive(false);
        }
        
        PlayerRotationSpeeds();
        PlayerRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        RotatingMesh();
    }

    private void FixedUpdate()
    {
        cameraSpeed = flyingStates.Speed + cameraSpeedOffset;

        flyingStates.rot.x += flyingStates.drop;

        flyingStates.CheckFlyingStates();
        ReduceAddVelocity();
        UpDraftCounter();

        if (flyingStates.isBoosting == false)
        {
            flyingStates.rb.velocity = flyingStates.baseVelocity + flyingStates.addedVelocity;
        }
        else if (flyingStates.isBoosting == true)
        {
            flyingStates.boostSpeed = Mathf.Lerp(0, flyingStates.boostSpeed, 1);
            flyingStates.baseVelocity += transform.forward * flyingStates.boostSpeed;
        }

        flyingStates.Speed = flyingStates.baseVelocity.magnitude;
        flyingStates.Speed = Mathf.Lerp(flyingStates.Speed, flyingStates.currentTargetSpeed, flyingStates.currentTargetForce * Time.deltaTime);
        flyingStates.baseVelocity = (flyingStates.rb.transform.forward * flyingStates.Speed);


        flyingStates.rb.velocity = flyingStates.baseVelocity + flyingStates.addedVelocity;


        flyingStates.TerminalBoost();
    }

    public void RotatingMesh()
    {

    }
    public void ReduceAddVelocity()
    {
        //Reduce the added velocity to 0 over time
        flyingStates.addedVelocity = Vector3.Lerp(flyingStates.addedVelocity, Vector3.zero, flyingStates.addedForceReduction * Time.deltaTime);

    }

    //Player Rotation------------------------------------Player Rotation------------------------------------Player Rotation------------------------------------
    public void PlayerRotationSpeeds()
    {
        if (flyingStates.Speed <= 100)
        {
            currentPitchRotationSpeed = Mathf.Lerp(currentPitchRotationSpeed, maxPitchSpeedRotation, PitchrotationChangeRate);
            currentYawRotationSpeed = Mathf.Lerp(currentYawRotationSpeed, maxYawSpeedRotation, YawrotationChangeRate);
        }
        else if (flyingStates.Speed >= 100)
        {
            currentPitchRotationSpeed = Mathf.Lerp(currentPitchRotationSpeed, minPitchSpeedRotation, PitchrotationChangeRate);
            currentYawRotationSpeed = Mathf.Lerp(currentYawRotationSpeed, minYawSpeedRotation, YawrotationChangeRate);
        }
    }
    public void PlayerRotation()
    {
        //Getting the angle that the glider is facing
        flyingStates.yAngle = flyingStates.rot.x;

        //adding the pitch inputs/data into the rot.x plus clamping the values
        flyingStates.rot.x += pitch;

        //max being the being first as when looking up the rotation is at its lowest, vise versa
        flyingStates.rot.x = Mathf.Clamp(flyingStates.rot.x, maxXAngle, minXAngle);


        flyingStates.rot.y += yaw;
        //rotating the glider by the rigidbody via the rot values


        flyingStates.rb.transform.rotation = Quaternion.Euler(flyingStates.rot);
    }



    //Interaction-------------------------------------------Interaction-------------------------------------------Interaction-------------------------------------------
    public void UpDraftCounter()
    {
        var addedSpeed = flyingStates.addedVelocity.magnitude;
        if (addedSpeed > maxCurrentSpeedInUpDraft)
        {
            flyingStates.baseVelocity *= upDraftForwardVelocity;
        }
    }

    public void WindMovePlayer(Vector3 _windStrength)
    {
        Debug.Log("Pushed by wind");
        flyingStates.addedVelocity += _windStrength;
    }


    //Camera Functions-------------------------------------Camera Functions-------------------------------------Camera Functions-------------------------------------
    public void CamFollow()
    {
        //3.5 0.75 close 7 2 far

        Camera.main.fieldOfView = Mathf.Abs(flyingStates.Speed / offSet + cameraSpeedOffset);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + Vector3.up * (camHeightDist);
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position);
    }

    //Debugging----------------------------------------------Debugging----------------------------------------------Debugging----------------------------------------------

    public void TestingKeys()
    {
        //Test the vertical Up Draft
        if (Input.GetKey(KeyCode.Space))
        {
            WindMovePlayer(Vector3.up * 100);
        }
    }
}
