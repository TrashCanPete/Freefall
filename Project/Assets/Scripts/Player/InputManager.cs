using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GliderController gliderController;
    private FlyingStates flyingStates;
    private RotationController rotationController;
    public AnimationScript animationScript;
    public AudioManager audioManager;
    public ParticleSystem boostPop;

    [SerializeField]
    private bool isRollingRight;
    [SerializeField]
    private bool canRollRight;

    [SerializeField]
    private float canRollRightCounter;
    [SerializeField]
    private float canRollRightTarget;
    [SerializeField]
    private float canRollRightStep;

    [SerializeField]
    private bool isRollingLeft;
    [SerializeField]
    private bool canRollLeft;

    [SerializeField]
    private float canRollLeftCounter;
    [SerializeField]
    private float canRollLeftTarget;
    [SerializeField]
    private float canRollLeftStep;

    public float minBoost;
    public float maxBoost;
    public float boostVariable;
    private float boostUpdater;

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
        canRollLeft = true;
        canRollRight = true;
        animationScript = GetComponent<AnimationScript>();
        rotationController = GetComponent<RotationController>();
        flyingStates = GetComponent<FlyingStates>();
        gliderController = GetComponent<GliderController>();
        audioManager = GetComponent<AudioManager>();
        
    }


    void Update()
    {
        boostVariable = boostUpdater;
        LeftRollCounter();
        RightRollCounter();
    }
    public void InputData()
    {
        yaw = yRotationSpeed * Input.GetAxis("Horizontal") * rotationController.currentYawRotationSpeed * Time.deltaTime;

        if (flyingStates.canTurnUp == true)
        {
            pitch = xRotationSpeed * Input.GetAxis("Vertical") * rotationController.currentPitchRotationSpeed * Time.deltaTime;
        }
        else if (flyingStates.canTurnUp == false)
        {
            pitch = Mathf.Clamp(pitch, 0, 1);
        }
        


        if (flyingStates.boostFuel <= 0)
        {
            Debug.Log("Out of fuel");
            if (Input.GetButtonDown("Shift"))
            {
                FindObjectOfType<AudioManager>().PlayAudio("OutOfFuel");
            }

            flyingStates.isBoosting = false;
            boostUpdater = minBoost;
            FindObjectOfType<AudioManager>().StopPlayingAudio("Boost");



        }
        else if (flyingStates.boostFuel > 0)
        {
            if (Input.GetButtonDown("Shift"))
            {
                flyingStates.isBoosting = true;
                boostUpdater = maxBoost;
                FindObjectOfType<AudioManager>().PlayAudio("Boost");
                boostPop.Play();
            }

            else if (Input.GetButtonUp("Shift"))
            {
                flyingStates.isBoosting = false;

                boostUpdater = minBoost;
                FindObjectOfType<AudioManager>().StopPlayingAudio("Boost");

            }

        }

        if (Input.GetButton("Roll_Right"))
        {
            if(canRollRight == true)
            {
                canRollRight = false;
                animationScript.rollRight = true;
                flyingStates.addedVelocity += transform.right * 150;
            }

        }
        else if (Input.GetButtonUp("Roll_Right"))
        {
            animationScript.rollRight = false;
        }

        if (Input.GetButton("Roll_Left"))
        {
            if(canRollLeft == true)
            {
                canRollLeft = false;
                animationScript.rollLeft = true;
                flyingStates.addedVelocity += -transform.right * 150;
            }

        }
        else if (Input.GetButtonUp("Roll_Left"))
        {
            animationScript.rollLeft = false;
        }
    }

    void RightRollCounter()
    { 
        
      if (canRollRight == false)
        {
            canRollRightCounter += canRollRightStep;
            if(canRollRightCounter >= canRollRightTarget)
            {
                canRollRight = true;
                canRollRightCounter = 0;
            }
        }
    }
    void LeftRollCounter()
    {
        if(canRollLeft == false)
        {
            canRollLeftCounter += canRollLeftStep;
            if(canRollLeftCounter >= canRollLeftTarget)
            {
                canRollLeft = true;
                canRollLeftCounter = 0;
            }
        }
    }
}
