using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GliderController gliderController;
    private FlyingStates flyingStates;

    //input variables
    [SerializeField]
    public float pitch;
    public float yaw;

    [SerializeField]
    private float xRotationSpeed;
    [SerializeField]
    private float yRotationSpeed;


    // Start is called before the first frame update
    void Start()
    {
        flyingStates = GetComponent<FlyingStates>();
        gliderController = GetComponent<GliderController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InputData()
    {
        //Getting the input data for rotatiing
        yaw = yRotationSpeed * Input.GetAxis("Horizontal") * gliderController.currentYawRotationSpeed * Time.deltaTime;

        if (flyingStates.canTurnUp == true)
        {
            pitch = xRotationSpeed * Input.GetAxis("Vertical") * gliderController.currentPitchRotationSpeed * Time.deltaTime;
        }
        else if (flyingStates.canTurnUp == false)
        {
            pitch = Mathf.Clamp(pitch, 0, 1);
            pitch = xRotationSpeed * Input.GetAxis("Vertical") * gliderController.currentPitchRotationSpeed * Time.deltaTime;
        }


        if (flyingStates.boostFuel == 0)
        {
            Debug.Log("Out of fuel");
            gliderController.boostLight.SetActive(false);
            flyingStates.isBoosting = false;

        }
        else if (flyingStates.boostFuel > 0)
        {
            if (Input.GetButtonDown("Shift"))
            {
                flyingStates.isBoosting = true;
                gliderController.boostLight.SetActive(true);
            }
            else if (Input.GetButtonUp("Shift"))
            {
                flyingStates.isBoosting = false;
                gliderController.boostLight.SetActive(false);
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
