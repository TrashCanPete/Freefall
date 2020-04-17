using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeMovement : MonoBehaviour
{

    public FlyingStates flyingStates;
    public Material capemovement;
    public float Windspeed;

    // Start is called before the first frame update
    void Start()
    {
        flyingStates = GetComponent < FlyingStates > ();
        capemovement.SetFloat("Vector1_f973097f",Windspeed);
    }

    // Update is called once per frame
    void Update()
    {
        _ = Windspeed == flyingStates.Speed;
         
    }
}
