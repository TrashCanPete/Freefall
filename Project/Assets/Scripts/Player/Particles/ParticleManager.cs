﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem wingBurstParticles;

    public ParticleSystem FadeInParticles;

    public FlyingStates flyingStates;

    // Start is called before the first frame update
    void Start()
    {
        wingBurstParticles = GetComponent<ParticleSystem>();
        FadeInParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        burstController();
        FadeInController();
    }



    void burstController()
    {

        var emission = wingBurstParticles.emission;
        emission.rateOverTime = flyingStates.burstState;
    }
    void FadeInController()
    {

        var emission = FadeInParticles.emission;
        emission.rateOverTime = flyingStates.fadeInState;
    }
}
