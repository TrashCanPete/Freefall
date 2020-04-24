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
    private GameObject thisGO;
    [SerializeField]
    private GliderController gliderController;

    [SerializeField]
    private SkinnedMeshRenderer plantRenderer;
    [SerializeField]
    public ParticleSystem spores;

    //public ParticleSystem plantPopPS;
    public AnimationScript animationScript;


    // Start is called before the first frame update
    void Start()
    {
        plantRenderer = GetComponent<SkinnedMeshRenderer>();
        gliderController = GetComponent<GliderController>();
        thisGO = this.gameObject;
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
            plantRenderer.enabled = false;
            StartCoroutine("WaitToDestroy");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Player"))
        {
        }
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
