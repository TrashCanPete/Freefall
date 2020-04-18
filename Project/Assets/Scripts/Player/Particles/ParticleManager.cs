using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem wingBurstParticles;

    public FlyingStates flyingStates;

    // Start is called before the first frame update
    void Start()
    {
        wingBurstParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WingBurstTimer()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
