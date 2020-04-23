using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockScript : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform target;
    public Transform target1;
    public Transform target2;

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
        if (other.tag == "Player")
        {
            Instantiate(birdPrefab, target.transform.position, Quaternion.identity);
            Instantiate(birdPrefab, target1.transform.position, Quaternion.identity);
            Instantiate(birdPrefab, target2.transform.position, Quaternion.identity);
        }
    }
}
