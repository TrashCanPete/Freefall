using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeMovement : MonoBehaviour
{

    public FlyingStates flyingStates;
    public Material capemovement;
    public float Windspeed;
    

   
    // Update is called once per frame
    void FixedUpdate()
    {

        capemovement.SetFloat("Vector1_f973097f", Windspeed);
    }
}

