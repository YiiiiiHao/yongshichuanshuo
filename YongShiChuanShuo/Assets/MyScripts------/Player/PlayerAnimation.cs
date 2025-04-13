using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{ 
    private Animator anim;
    private Rigidbody2D rb;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {   //rb.velocity.x速度是多少，velosityX的值就是多少,
        //由于在animatro中是大于0.1就跑步，但是向左跑会是-1，所以用Mathf.Abs(rb.velocity.x)来获取绝对值
        //这样向左向右跑就是大于0.1.
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
    
    }
}
