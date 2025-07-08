using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;//跳跃动作需要知道是否在地面上，所以需要获取PhysicsCheck组件里的isGround属性
    private PlayerController playerController;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
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
        //根据y轴速度来设置跳跃动画
        anim.SetFloat("velocityY", rb.velocity.y);//y轴速度，跳跃
        anim.SetBool("isGround", physicsCheck.isGround);//是否在地面上
        anim.SetBool("isCrouch", playerController.isCrouch);//是否处于蹲下状态
    }

    public void PlayHurt()
    {
        anim.SetTrigger("Hurt");//这里不要拼写错误
    }
}
