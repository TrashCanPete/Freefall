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
    public bool enteredUpDraft;
    public GliderController _gliderController;

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

    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;

    private void Start()
    {
        _gliderController = this;
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
            currentTargetSpeed = standardMaxVelocity;
            currentTargetForce = standardForce;
        }
        //Diving
        else if (yAngle >= diveThreshold)
        {
            Debug.Log("Diving");
            currentTargetSpeed = divingMaxVelocity;
            currentTargetForce = divingForce;

        }
        //Risinig
        else if (yAngle <= riseThreshold)
        {
            Debug.Log("Rising");
            currentTargetSpeed = risingMaxVelocity;
            currentTargetForce = risingForce;
        }


        Speed = baseVelocity.magnitude;
        Speed = Mathf.Lerp(Speed, currentTargetSpeed, currentTargetForce * Time.deltaTime);
        baseVelocity = (rb.transform.forward * Speed);

        if (Input.GetKey(KeyCode.Space))
        {
            WindMovePlayer(100);
        }
        
        //Reduce the added velocity to 0 over time
        addedVelocity = Vector3.Lerp(addedVelocity, Vector3.zero, addedForceReduction * Time.deltaTime);
        var addedSpeed = addedVelocity.magnitude;

        if (addedSpeed > maxCurrentSpeedInUpDraft)
        {
            baseVelocity *= upDraftForwardVelocity;
        }

        rb.velocity = baseVelocity + addedVelocity;
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


    public void WindMovePlayer(float _windStrength)
    {
        Debug.Log("Pushed by wind");
        addedVelocity = Vector3.up * _windStrength;
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
