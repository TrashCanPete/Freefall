using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockScript : MonoBehaviour
{
    public GameObject birdPrefab1;
    public GameObject birdPrefab2;
    public GameObject birdPrefab3;
    public Transform target;
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;


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
            Instantiate(birdPrefab1, target.transform.position, Quaternion.identity);
            Instantiate(birdPrefab2, target1.transform.position, Quaternion.identity);
            Instantiate(birdPrefab3, target2.transform.position, Quaternion.identity);
            Instantiate(birdPrefab1, target3.transform.position, Quaternion.identity);
            Instantiate(birdPrefab1, target4.transform.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
