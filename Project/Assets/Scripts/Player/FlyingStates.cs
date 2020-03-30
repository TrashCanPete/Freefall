﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStates : MonoBehaviour
{
    // Get the player Rigidbody component
    public Rigidbody rb;
    // Rotation
    public Vector3 rot;

    public GameObject wingsOut;
    public GameObject wingsIn;

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


    public float drop;


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


    [SerializeField]
    private float standardForce;
    [SerializeField]
    private float divingForce;
    [SerializeField]
    private float risingForce;

    public float addedForceReduction;

    [SerializeField]
    private float terminalForce;
    [SerializeField]
    private float maxRisingForce;

    //Angles--------------------------------------------------Angles--------------------------------------------------Angles--------------------------------------------------
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

    //Boost-------------------------------------------------//Boost-------------------------------------------------//Boost-------------------------------------------------

    [SerializeField]
    private float terminalBoostSpeed;

    public float boostSpeed;
    
    public bool isBoosting = false;

    [SerializeField]
    private float maxBoostFuel;
    [SerializeField]
    private float fuelConsumption;




    //Start
    private void Start()
    {
        //wings out
        wingsIn.SetActive(false);
        wingsOut.SetActive(true);


        boostFuel = maxBoostFuel;
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
                //wings in
                wingsIn.SetActive(true);
                wingsOut.SetActive(false);
                if (isInTerminalVelocity == true)
                {
                    canTerminalBoost = true;
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
    //End of Method

    //Boost using Terminal Velocity-------------------Boost using Terminal Velocity-------------------Boost using Terminal Velocity-------------------
    public void TerminalBoost()
    {
        if (yAngle <= diveThreshold)
        {
            isInTerminalVelocity = false;

            if (!canTerminalBoost)
            {
                Debug.Log("canBoost is false");
            }
            else if (canTerminalBoost == true)
            {
                //wings out
                wingsIn.SetActive(false);
                wingsOut.SetActive(true);
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
}
