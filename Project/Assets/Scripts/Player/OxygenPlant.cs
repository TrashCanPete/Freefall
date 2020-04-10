using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPlant : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private float windStrength;
    [SerializeField]
    private int oxygen;
    [SerializeField]
    private FlyingStates flyingStates;

    public AnimationScript animationScript;


    // Start is called before the first frame update
    void Start()
    {
        flyingStates = GetComponent<FlyingStates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            other.GetComponent<GliderController>().OxygenPlantPush ( windStrength ,oxygen);
            Destroy(gameObject);

        }
    }
}
