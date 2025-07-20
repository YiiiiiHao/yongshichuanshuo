using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    [Header("转身计时器")]
    public float waitTime;
    public float waitTimeCounter=0;
    public bool wait;
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
        }
        TimeCounter();
    }
    void FixedUpdate()
    {
        Move();//刚体移动，所以放在FixedUpdate中
    }

    public virtual void Move()
    {
        //速度*方向*时间。Y方向不动
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

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
}
