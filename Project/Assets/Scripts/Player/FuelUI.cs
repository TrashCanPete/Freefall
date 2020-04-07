using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public FlyingStates flyingStates;
    [SerializeField]
    private Slider fuel;
    // Start is called before the first frame update
    void Start()
    {
        fuel = GetComponent<Slider>();
        fuel.maxValue = flyingStates.maxBoostFuel;
        fuel.value = flyingStates.boostFuel;
    }

    // Update is called once per frame
    void Update()
    {
        fuel.value = flyingStates.boostFuel;
    }
}
