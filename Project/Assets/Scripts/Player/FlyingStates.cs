using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStates : MonoBehaviour
{
    // Get the player Rigidbody component
    public Rigidbody rb;
    // Rotation
    public Vector3 rot;


    public bool canTurnUp;

    public GameObject wingsObj;

    //Windstream variables
    public float streamOn = 4;
    public float streamOff = 0;
    public float streamState;

    public bool wingsOut;

    public float burstState;
    public float burstOn;
    public float burstOff;

    public float fadeInState;
    public float fadeInOn;
    public float fadeInOff;


    public bool burstEm;
    public bool fadeInEm;

    public ParticleManager burstParticleManager;
    public ParticleManager fadeInParticleManager;


    //basic variable trackers------------------------------basic variable trackers------------------------------basic variable trackers------------------------------
    [Header("Basic Variables")]
    public float boostFuel;

    public float yAngle;
    public float Speed;

    public float currentTargetSpeed;
    public float currentTargetForce;

    private bool High;
    private bool Mid;
    private bool Low;

    [SerializeField]
    private bool isInTerminalVelocity;
    [SerializeField]
    private bool canTerminalBoost;

    [SerializeField]
    private float minDrop;
    [SerializeField]
    private float maxDrop;
    public float drop;
    [SerializeField]
    private float dropChange;


    //gliders velocity variables----------------------------gliders velocity variables----------------------------gliders velocity variables----------------------------
    [Header("Velocity")]
    public Vector3 baseVelocity;
    public Vector3 addedVelocity;

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

    //standard force
    [SerializeField]
    private float standardForce;

    [SerializeField]
    private float divingForce;
    [SerializeField]
    private float terminalForce;

    [SerializeField]
    private float risingForce;
    [SerializeField]
    private float maxRisingForce;

    public float addedForceReduction;




    //Angles--------------------------------------------------Angles--------------------------------------------------Angles--------------------------------------------------
    [Header("Angle Variables")]
    [SerializeField]
    private float diveThreshold;
    [SerializeField]
    private float terminalThreshold;
    [SerializeField]
    private float riseThreshold;


    //Counters----------------------------------------------//Counters----------------------------------------------//Counters----------------------------------------------
    [Header("Counters")]
    //diving
    [SerializeField]
    private float maxDivingCounter;
    [SerializeField]
    private float divingCounterRate;
    [SerializeField]
    private float divingCounterStep;


    //terminal velocity dive
    [SerializeField]
    private float maxTerminalDivingCounter;
    [SerializeField]
    private float terminalDivingRateCounter;
    [SerializeField]
    private float terminalDivingStepCounter;


    //rising
    [SerializeField]
    private float maxRisingCounter;
    [SerializeField]
    private float risingCounterRate;
    [SerializeField]
    private float risingCounterStep;

    //rising twist
    [SerializeField]
    private float maxRisingTwistCounter;
    [SerializeField]
    private float risingTwistRate;
    [SerializeField]
    private float risingTwistStep;

    //Boost-------------------------------------------------//Boost-------------------------------------------------//Boost-------------------------------------------------

    [SerializeField]
    private float terminalBoostSpeed;

    public float boostSpeed;
    
    public bool isBoosting = false;

    public float maxBoostFuel;

    [SerializeField]
    private float fuelConsumption;




    //Start
    private void Start()
    {
        burstEm = false;
        fadeInEm = false;
        canTurnUp = true;
        //wings out
        WingsOut();
        wingsOut = true;

        boostFuel = maxBoostFuel / 2;
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;
    }

    private void Update()
    {
        boostFuel = Mathf.Clamp(boostFuel, 0, maxBoostFuel);
    }
    
    //Start of Method
    public void CheckFlyingStates()
    {
        //Standard------------------------
        if (yAngle <= diveThreshold && yAngle >= riseThreshold)
        {
            //High = false;
            //Mid = true;
            //Low = false;

            ResetSpeedAndForceValues();

            risingCounterRate = 0;
            divingCounterRate = 0;
            drop = Mathf.Lerp(drop, minDrop, 0.25f);
            canTurnUp = true;

            if (divingCounterRate == 0)
            {
                ResetSpeedAndForceValues();
            }
        }
        //Diving------------------------------------------
        else if (yAngle >= diveThreshold)
        {
            //High = false;
            //Mid = false;
            //Low = true;

            currentTargetSpeed = divingMaxVelocity;
            currentTargetForce = divingForce;

            if (yAngle >= terminalThreshold)
            {
                divingCounterRate += divingCounterStep;

                if (divingCounterRate >= maxDivingCounter)
                {
                    currentTargetSpeed = terminalVelocity;
                    currentTargetForce = terminalForce;
                    terminalDivingRateCounter += terminalDivingStepCounter;
                    if (terminalDivingRateCounter >= terminalDivingStepCounter)
                    {
                        //wings in
                        WingsIn();
                        wingsOut = false;

                        isInTerminalVelocity = true;

                    }
                    if (isInTerminalVelocity == true)
                    {
                        canTerminalBoost = true;
                    }
                }
            }
        }
        //Rising-----------------------------------
        else if (yAngle <= riseThreshold)
        {
            //High = true;
            //Mid = false;
            //Low = false;

            currentTargetSpeed = risingMaxVelocity;
            currentTargetForce = risingForce;
            risingCounterRate += risingCounterStep;

            if (risingCounterRate >= maxRisingCounter)
            {
                Debug.Log("Max Climb");
                currentTargetSpeed = maxRisingVelocity;
                currentTargetForce = maxRisingForce;

                risingTwistRate += risingTwistStep;
                if (risingTwistRate >= maxRisingTwistCounter)
                {
                    if (isBoosting == true)
                    {
                        Debug.Log("Let it Rise");
                    }
                    else if (isBoosting == false)
                    {
                        drop += dropChange;
                        drop = Mathf.Clamp(drop, minDrop, maxDrop);
                        if (drop >= 2)
                        {
                            canTurnUp = false;
                        }
                    }

                }
            }
        }
    }
    //End of Method

    //Boost using Terminal Velocity-------------------Boost using Terminal Velocity-------------------Boost using Terminal Velocity-------------------
    public void TerminalBoost()
    {
        if (yAngle <= diveThreshold)
        {
            isInTerminalVelocity = false;

            if (!canTerminalBoost)
            {
            }
            else if (canTerminalBoost == true)
            {
                //wings out
                WingsOut();
                wingsOut = true;

                baseVelocity += transform.forward * terminalBoostSpeed;
                canTerminalBoost = false;
            }
        }

    }
    //Reset--------------------------------------------Reset--------------------------------------------Reset--------------------------------------------
    public void ResetSpeedAndForceValues()
    {
        currentTargetSpeed = standardMaxVelocity;
        currentTargetForce = standardForce;
    }


    public void UseBoosFuel()
    {
        if (isBoosting == false)
        {
            return;
        }
        else if (isBoosting == true)
        {
            boostFuel -= fuelConsumption;
        }
    }





    public void WingsIn()
    {
        wingsOut = false;
        if (wingsOut == true)
        {

        }
        else if (wingsOut == false)
        {
            wingsObj.SetActive(false);
            WingStreamsOff();
            fadeInState = fadeInOn;
            burstState = burstOn;
            //StartCoroutine("WingFadeInTimer");
        }



    }

    public IEnumerator WingFadeInTimer()
    {
        Debug.Log("FadeinStart");
        fadeInEm = true;
        fadeInParticleManager.FadeInParticles.Play();

        yield return new WaitForSeconds(1f);
        Debug.Log("FadeinStop");
        fadeInEm = false;
        fadeInParticleManager.FadeInParticles.Stop();
        //fadeInState = fadeInOff;



        StartCoroutine("WingBurstTimer");
    }

    public IEnumerator WingBurstTimer()
    {
        burstParticleManager.wingBurstParticles.Play();
        Debug.Log("BurstStart");
        burstEm = true;
        yield return new WaitForSeconds(1f);
        burstParticleManager.wingBurstParticles.Stop();
        burstEm = false;
    }




    public void WingsOut()
    {
        wingsOut = true;
        if (wingsOut == false)
        {
        }
        else if (wingsOut == true)
        {
            wingsObj.SetActive(true);
            WingStreamsOn();
        }


    }



    public void WingStreamsOff()
    {
        streamState = streamOff;
    }


    public void WingStreamsOn()
    {
        streamState = streamOn;
    }





}
