using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBoost : MonoBehaviour
{
    public ParticleSystem ps;

    public InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var emission = ps.emission;
        emission.rateOverTime = inputManager.boostVariable;
    }
}
