using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GliderController : MonoBehaviour
{
    public Transform StartPosition;
    public Transform playerForward;
    [SerializeField]
    private GameObject windStream;
    [SerializeField]
    public enum _collison {UpDraft, Oxygen }
    public GameObject boostLight;
    public GameObject meshGrp;





    //rotation variables--------------------------------------rotation variables--------------------------------------rotation variables--------------------------------------
    [Header("Rotation Variables")]

    [SerializeField]
    private float minXAngle;
    [SerializeField]
    private float maxXAngle;

    [Header("Yaw Speeds - Up and Down")]
    //Yaw-------------------------
    public float currentYawRotationSpeed;
    [SerializeField]
    private float maxYawSpeedRotation;
    [SerializeField]
    private float minYawSpeedRotation;
    [SerializeField]
    private float YawrotationChangeRate;

    [Header("Pitch Speeds - Left and Right")]
    //Pitch--------------------
    public float currentPitchRotationSpeed;
    [SerializeField]
    private float maxPitchSpeedRotation;
    [SerializeField]
    private float minPitchSpeedRotation;
    [SerializeField]
    private float PitchrotationChangeRate;


    //Counters----------------------------------------------//Counters----------------------------------------------//Counters----------------------------------------------



    private float colliderXIn = 0.3f;
    private float colliderXOut = 2.4f;
    private BoxCollider playerCollider;


    //UpDraft Variables------------------------------------UpDraft Variables------------------------------------UpDraft Variables------------------------------------
    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;
    [SerializeField]
    private float oxygenPlantBoost;

    //public scripts
    public DebugLines debugLines;
    public FlyingStates flyingStates;
    private CamFollow camFollow;
    private InputManager input;



    //Start
    private void Start()
    {

        transform.position = StartPosition.transform.position;
        //Calling Scripts
        debugLines = GetComponent<DebugLines>();
        flyingStates = GetComponent<FlyingStates>();
        playerCollider = GetComponent<BoxCollider>();
        camFollow = GetComponent<CamFollow>();
        input = GetComponent<InputManager>();

        boostLight.SetActive(false);
        windStream.SetActive(false);


    }

    private void Update()
    {

        //drawing lines
        debugLines.DebugDrawLines();

        camFollow.CameraFollow();

        input.InputData();

        PlayerRotationSpeeds();
        PlayerRotation();


        RotatingMesh();

        if (transform.position.y <= 0)
        {
            transform.position = StartPosition.transform.position;
        }

    }

    private void FixedUpdate()
    {
        flyingStates.rot.x += flyingStates.drop;

        flyingStates.CheckFlyingStates();
        ReduceAddVelocity();

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
        if (flyingStates.rb.velocity.magnitude >= 75f)
        {
            windStream.SetActive(true);
        }
        else if (flyingStates.rb.velocity.magnitude <= 125f) 
        { 
            windStream.SetActive(false); 
        }

        flyingStates.TerminalBoost();
        flyingStates.UseBoosFuel();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("UpDraft"))
        {
            Debug.Log("Col Trigger UpDraft");
            UpDraftCounter(_collison.UpDraft);
        }
        else if (other.tag == ("Oxygen"))
        {
            UpDraftCounter(_collison.Oxygen);
        }
    }

    public void RotatingMesh()
    {
        var verticalRollValue = 10;
        var horizontalRollValue = 10;

        var verticalRoll = input.pitch * verticalRollValue;
        var horizontalRoll = input.yaw * -horizontalRollValue;

        verticalRoll = Mathf.Clamp(verticalRoll, -verticalRollValue, verticalRollValue);
        horizontalRoll = Mathf.Clamp(horizontalRoll, -horizontalRollValue + 1, horizontalRollValue - 1);

        meshGrp.transform.localRotation = Quaternion.RotateTowards(meshGrp.transform.localRotation, Quaternion.Euler(verticalRoll, 0, horizontalRoll), 50.0f * Time.deltaTime);
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
        flyingStates.rot.x += input.pitch;

        //max being the being first as when looking up the rotation is at its lowest, vise versa
        flyingStates.rot.x = Mathf.Clamp(flyingStates.rot.x, maxXAngle, minXAngle);


        flyingStates.rot.y += input.yaw;
        //rotating the glider by the rigidbody via the rot values


        flyingStates.rb.transform.rotation = Quaternion.Euler(flyingStates.rot);
    }



    //Interaction-------------------------------------------Interaction-------------------------------------------Interaction-------------------------------------------
    public void UpDraftCounter(_collison col)
    {
        var addedSpeed = flyingStates.addedVelocity.magnitude;

       if (col == _collison.UpDraft)
        {
            Debug.Log("Col UpDraft");
            if (addedSpeed > maxCurrentSpeedInUpDraft)
            {
                //Slows you down
                Debug.Log("Col UpDraft Inside");
                flyingStates.baseVelocity *= (0.5f * upDraftForwardVelocity);
            }
        }
        else if (col == _collison.Oxygen)
        {
            //Slows you down
            Debug.Log("Col Oxygen");
            flyingStates.baseVelocity *= (0.5f * oxygenPlantBoost);
        }
        
    }

    public void WindMovePlayer(Vector3 _windStrength)
    {
        Debug.Log("Pushed by wind");
        flyingStates.addedVelocity += _windStrength;
    }
    public void OxygenPlantPush( float _OxygenBoostStrength, int _AddOxygen)
    {
        flyingStates.addedVelocity += (transform.forward * _OxygenBoostStrength);
        flyingStates.boostFuel += _AddOxygen;
    }



    //Debugging----------------------------------------------Debugging----------------------------------------------Debugging----------------------------------------------

    public void TestingKeys()
    {
        //Test the vertical Up Draft
        if (Input.GetKey(KeyCode.Space))
        {
            WindMovePlayer(Vector3.up * 100);
            WindMovePlayer(Vector3.up * 100);
        }
    }

}
