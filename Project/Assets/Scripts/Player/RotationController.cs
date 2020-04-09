using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
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


    public GameObject meshGrp;

    private InputManager input;
    private FlyingStates flyingStates;


    // Start is called before the first frame update
    void Start()
    {
        flyingStates = GetComponent<FlyingStates>();
        input = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
