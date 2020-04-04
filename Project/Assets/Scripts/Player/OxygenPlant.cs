using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPlant : MonoBehaviour
{
    [SerializeField]
    private float windStrength;
    [SerializeField]
    private int oxygen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            other.GetComponent<GliderController>().OxygenPlantPush(Vector3.up * windStrength, oxygen);
            Destroy(gameObject);
        }
    }
}
