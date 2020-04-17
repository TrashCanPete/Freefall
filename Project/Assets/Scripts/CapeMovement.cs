using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeMovement : MonoBehaviour
{

    private FlyingStates flyingStates;
    public Material capemovement;
    [SerializeField]
    private float Windspeed;
    [SerializeField]
    private float windSpeedDivide;


    private void Start()
    {
        flyingStates = GetComponent<FlyingStates>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Windspeed = flyingStates.Speed;
        capemovement.SetFloat("Vector1_6AEB6D23", (Windspeed / windSpeedDivide ));
    }
}

