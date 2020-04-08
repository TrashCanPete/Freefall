using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public FlyingStates flyingStates;



    //Camera follow distance
    [Header("Cam Variables")]

    public Transform lookAtTransform;

    [SerializeField]
    private float lookAtOffsetX;
    [SerializeField]
    private float lookAtOffsetY;
    [SerializeField]
    private float lookAtOffsetZ;

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
    private float cameraSpeed;
    [SerializeField]
    private float cameraSpeedOffset;




    // Start is called before the first frame update
    void Start()
    {
        lookAtTransform.transform.position = new Vector3(lookAtOffsetX, lookAtOffsetY, lookAtOffsetZ);
        flyingStates = GetComponent<FlyingStates>();
    }

    private void FixedUpdate()
    {
        cameraSpeed = flyingStates.Speed + cameraSpeedOffset;
    }

    public void CameraFollow()
    {
        //3.5 0.75 close 7 2 far

        Camera.main.fieldOfView = Mathf.Abs(flyingStates.Speed / offSet + cameraSpeedOffset);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
        Vector3 moveCamTo = transform.position - transform.forward * camFollowDist + transform.up * (camHeightDist);
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(lookAtTransform.transform.position, Vector3.up);
    }
}
