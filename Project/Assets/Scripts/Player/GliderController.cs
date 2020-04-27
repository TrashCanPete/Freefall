using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GliderController : MonoBehaviour
{
    public Transform StartPosition;
    [SerializeField]
    private GameObject windStream;
    [SerializeField]
    public enum _collison {UpDraft, Oxygen }
    public GameObject boostLight;
    public GameObject EndGameUI;



    private float colliderXIn = 0.3f;
    private float colliderXOut = 2.4f;
    private BoxCollider playerCollider;

    private float oxygenBoostStrength;


    //UpDraft Variables------------------------------------UpDraft Variables------------------------------------UpDraft Variables------------------------------------
    [Header("Up Draft Variables")]

    [SerializeField]
    private float upDraftForwardVelocity;
    [SerializeField]
    private float maxCurrentSpeedInUpDraft;
    [SerializeField]
    private float oxygenPlantBoost;

    //public scripts
    public DebugLines debugLines;
    public FlyingStates flyingStates;
    private CamFollow camFollow;
    private InputManager input;
    private RotationController rotationController;
    private AnimationScript animationScript;
    private LoadLevel loadLevel;

    public AudioManager audioManager;
    public ParticleSystem plantPopPS;

    private bool isPlayingAudio;
    private AudioManager audio;

    public bool inUpDraft;

    private Vector3 updraftDirection;
    public Vector3 currentSpeed;

    [SerializeField]
    private float slowFallMultiplier;

    //Start
    private void Start()
    {
        inUpDraft = false;
        audio = FindObjectOfType<AudioManager>();

        //Calling Scripts
        debugLines = GetComponent<DebugLines>();
        flyingStates = GetComponent<FlyingStates>();
        playerCollider = GetComponent<BoxCollider>();
        camFollow = GetComponent<CamFollow>();
        input = GetComponent<InputManager>();
        rotationController = GetComponent<RotationController>();
        animationScript = GetComponent<AnimationScript>();
        loadLevel = GetComponent<LoadLevel>();

        //boostLight.SetActive(false);
        //windStream.SetActive(false);
        flyingStates.WingStreamsOff();

        EndGameUI.SetActive(false);

    }

    private void Update()
    {

        //drawing lines
        debugLines.DebugDrawLines();

        camFollow.CameraFollow();

        input.InputData();

        rotationController.PlayerRotationSpeeds();
        rotationController.PlayerRotation();


        rotationController.RotatingMesh();

        if (transform.position.y <= 0)
        {
            transform.position = StartPosition.transform.position;
        }

    }

    private void FixedUpdate()
    {
        flyingStates.rot.x += flyingStates.drop;

        flyingStates.CheckFlyingStates();
        ReduceAddVelocity();

        if (flyingStates.isBoosting == false)
        {
            flyingStates.rb.velocity = flyingStates.baseVelocity + flyingStates.addedVelocity;
        }
        else if (flyingStates.isBoosting == true)
        {
            flyingStates.boostSpeed = Mathf.Lerp(0, flyingStates.boostSpeed, 1);
            flyingStates.baseVelocity += transform.forward * flyingStates.boostSpeed;
        }

        flyingStates.Speed = flyingStates.baseVelocity.magnitude;
        flyingStates.Speed = Mathf.Lerp(flyingStates.Speed, flyingStates.currentTargetSpeed, flyingStates.currentTargetForce * Time.deltaTime);
        flyingStates.baseVelocity = (flyingStates.rb.transform.forward * flyingStates.Speed);


        flyingStates.rb.velocity = flyingStates.baseVelocity + flyingStates.addedVelocity;


        if (flyingStates.rb.velocity.magnitude >= 100f)
        {

            if (!isPlayingAudio)
            {
                audio.PlayAudio("FlyingFast");
                isPlayingAudio = true;
            }

            if (flyingStates.wingsOut == false)
            {
            }
            else if (flyingStates.wingsOut == true)
            {
                flyingStates.WingStreamsOn();
            }
        }

        else
        {
            audio.StopPlayingAudio("FlyingFast");
            isPlayingAudio = false;
            if (flyingStates.wingsOut == false)
            {

            }
            else if (flyingStates.wingsOut == true)
            {
                flyingStates.WingStreamsOff();
            }
        }

        flyingStates.TerminalBoost();
        flyingStates.UseBoosFuel();

        if (inUpDraft == true)
        {
            currentSpeed = flyingStates.rb.velocity;
            var upDraftBoost = 3;
            for (int i = 0; i < upDraftBoost; i++)
            {
                flyingStates.addedVelocity += (updraftDirection);
            }
        }

        flyingStates.rb.velocity += -Vector3.up * slowFallMultiplier;



    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("UpDraft"))
        {
            Debug.Log("Col Trigger UpDraft");
            UpDraftCounter(_collison.UpDraft);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Finish"))
        {
            Debug.Log("Finish");
            StartCoroutine("WaitToEndGame");
        }
        else if (other.tag == ("Oxygen"))
        {
            UpDraftCounter(_collison.Oxygen);

            FindObjectOfType<AudioManager>().PlayAudio("Pop");
        }
        else if (other.tag == ("UpDraft"))
        {
            inUpDraft = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("UpDraft"))
        {
            inUpDraft = false;
        }
    }
    private IEnumerator WaitToEndGame()
    {
        EndGameUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        EndGameUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    


    public void ReduceAddVelocity()
    {
        //Reduce the added velocity to 0 over time
        flyingStates.addedVelocity = Vector3.Lerp(flyingStates.addedVelocity, Vector3.zero, flyingStates.addedForceReduction * Time.deltaTime);
    }

    //Interaction-------------------------------------------Interaction-------------------------------------------Interaction-------------------------------------------
    public void UpDraftCounter(_collison col)
    {
        var addedSpeed = flyingStates.addedVelocity.magnitude;

       if (col == _collison.UpDraft)
        {
            if (addedSpeed > maxCurrentSpeedInUpDraft)
            {
                //Slows you down
                flyingStates.baseVelocity *= (0.5f * upDraftForwardVelocity);
            }

        }
        else if (col == _collison.Oxygen)
        {
            //Slows you down
            //flyingStates.baseVelocity *= (0.5f * oxygenPlantBoost);
        }
        
    }

    public void WindMovePlayer(Vector3 _windStrength)
    {
        updraftDirection = _windStrength;
        //flyingStates.addedVelocity += _windStrength;
    }
    public void OxygenPlantPush( float _OxygenBoostStrength, int _AddOxygen)
    {

        flyingStates.boostFuel += _AddOxygen;
    }

    void PlantAddSpeed()
    {
        flyingStates.addedVelocity += (transform.forward * oxygenBoostStrength);
    }

}
