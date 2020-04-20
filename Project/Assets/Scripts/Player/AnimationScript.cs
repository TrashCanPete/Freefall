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
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        //Calling blend tree function
        Move(x, y);

        if (Input.GetButtonDown("Shift"))
        {
            
            anim.SetBool("Boosting", true);
        }

        else if (Input.GetButtonUp("Shift"))
        {
            anim.SetBool("Boosting", false);
        }

    }

    //Blend tree Function
    public void Move(float x, float y)
    {
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);
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

    /*
    public void BoostingAnimation()
    {
        Debug.Log("Boosting Animation");
        if (Input.GetButtonDown("Shift"))
        {
            anim.SetBool("Boosting", true);   
        }

        else if (Input.GetButtonUp("Shift"))
        {
            anim.SetBool("Boosting", false);
        }
    }
    */
    public void PlayerOxygenPlantAnimation()
    {
        
    }

}
