using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTreeAnimation : MonoBehaviour
       
{
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim = null) return;
        
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        Move(x, y);

    }

    public void Move(float x, float y)
    {
        anim.SetFloat("Velx", x);
        anim.SetFloat("Vely", y);
    }
}
