using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTreeAnimation : MonoBehaviour
       
{
    public Animator anim;
    private InputManager inputManager;

    private float y;
    private float x;


    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (anim = null) return;
        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        Move(x, y);

    }

    public void Move(float x, float y)
    {
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);
    }
}
