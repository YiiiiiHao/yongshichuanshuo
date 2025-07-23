using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;//正常速度
    public float chaseSpeed;//追逐速度
    public float currentSpeed;//当前速度
    public float hurtForce;//受伤后弹开的力量
    public Vector3 faceDir;
    [Header("转身计时器")]
    public float waitTime;
    public float waitTimeCounter = 0;
    public bool wait;
    public Transform attacker;
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//设置朝向

        //撞墙转身
        if ((physicsCheck.touchRightWall && faceDir.x > 0) || (physicsCheck.touchLeftWall && faceDir.x < 0))
        {
            wait = true;
            anim.SetBool("Walk", false);
        }
        else
        {
            wait = false;
            anim.SetBool("Walk", true);
        }
        TimeCounter();
    }
    void FixedUpdate()
    {
        if (!isHurt && !isDead)//不受伤 且 未死亡 才能移动
        {
            Move();//刚体移动，所以放在FixedUpdate中
        }
    }

    public virtual void Move()
    {
        //速度*方向*时间。Y方向不动
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;//设置的时间，减去时间修正。
        }
        if (waitTimeCounter <= 0)
        {
            wait = false;
            waitTimeCounter = waitTime;//时间归0
            transform.localScale = new Vector3(faceDir.x, 1, 1);
        }
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;//攻击者的transform转移到被攻击者。
        //转身
        if (attackTrans.position.x - transform.position.x > 0)//
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        //受伤被击退
        isHurt = true;

        anim.SetTrigger("Hurt");
        Vector2 Dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        StartCoroutine(OnHurt(Dir));//  调用携程固定写法
     }
    IEnumerator OnHurt(Vector2 Dir)
    {
        rb.AddForce(Dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);   //等待0.5秒
        isHurt = false;

    }
    public void OnDead()
    {
        gameObject.layer = 2;
        anim.SetBool("Dead", true);
        isDead = true;
    }

    public void OnDestroyAfterAnimation()
    {
         Destroy(this.gameObject);
    }
}
