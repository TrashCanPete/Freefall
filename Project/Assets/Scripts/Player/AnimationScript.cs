using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public Animator anim;
    public AnimationScript animationScript;
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        animationScript = GetComponent<AnimationScript>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForAnimations()
    {

    }
    public void TiltingAnimation()
    {
        if (inputManager.pitch > 0)
        {
            Debug.Log("Diving Animation");
            anim.SetBool("Turn_Down", true);
        }
        else if (inputManager.pitch < 0)
        {
            Debug.Log("Rising Animation");
            anim.SetBool("Turn_Up", true);
        }
        else if (inputManager.pitch == 0) 
        {
            anim.SetBool("Turn_Down", false);
            anim.SetBool("Turn_Up", false);
        }

    }

    public void TurningAnimation()
    {
        if (inputManager.yaw > 0)
        {
            Debug.Log("Right Turning Animation");
            anim.SetBool("Turn_Right", true);
        }
        else if (inputManager.yaw < 0)
        {
            Debug.Log("Left Turning Animation");
            anim.SetBool("Turn_Left", true);
        }
        else if (inputManager.yaw == 0)
        {
            anim.SetBool("Turn_Right", false);
            anim.SetBool("Turn_Left", false);
        }

    }

    public void BoostingAnimation()
    {
        Debug.Log("Boosting Animation");
    }

    public void PlayerOxygenPlantAnimation()
    {
        
    }

}
