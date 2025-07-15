using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;//跳跃动作需要知道是否在地面上，所以需要获取PhysicsCheck组件里的isGround属性
    private PlayerController playerController;
    private Character character;
    void Awake()
    {
        anim = GetComponent<Animator>();//获取animator组件
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        character = GetComponent<Character>();
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
        anim.SetBool("Dead", playerController.isDead);//是否死亡
        anim.SetBool("isAttack", playerController.isAttack);//是否攻击
    }

    public void PlayHurt()//此脚本在Character脚本中，被（事件）调用
    {
        anim.SetTrigger("Hurt");//这里不要拼写错误
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");//bool用来控制动画播放，Trigger用来触发动画事件
    }
}
