using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;//获取角色控制器
    public PhysicsCheck physicsCheck;//获取角色的物理检测组件

    //要用Vector2的值来驱动RIgidbody2D的Velocity属性。
    public Vector2 inputDirection;//用于读取PlayerInputControl的MOve里的Vector2值 在Unity显示为-1 ~ 1
    public Rigidbody2D rb;//用于驱动Rigidbody2D的Velocity属性
    [Header("基本参数")]
    public float speed = 5f;  //速度
    public float jumpForce = 10f;

    //Jump

    private void Awake()
    {
        inputControl = new PlayerInputControl(); //实例化PlayerInputControl类
        // rb = GetComponent<Rigidbody2D>(); //获取Rigidbody2D组件

        //staerted:按下按键瞬间则启用
        //按下空格键和右键（在PlayerInputControl里设置）时，调用Jump方法
        inputControl.GamePlay.Jump.started += Jump;
        physicsCheck = GetComponent<PhysicsCheck>(); //获取PhysicsCheck组件
        
    }



    void OnEnable()
    {
        inputControl.Enable(); //启用PlayerInputControl类
    }
    void OnDisable()
    {
        inputControl.Dispose(); //销毁PlayerInputControl类
    }

    void Update()
    {   //inputDirection用于读取PlayerInputControl的Move里的Vector2值,ReadValue<Vector2>()方法用于读取输入值
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>(); //读取PlayerInputControl的Move里的Vector2值
    }

    void FixedUpdate()//方法用于驱动Rigidbody2D的Velocity属性
    {
        Move();
        //     if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Jump(); 
        // }
    }
    public void Move()
    {
        //如果是星露谷物语那种，可以用下面这行代码，并将RIgidbody2D的gravity属性设置为0，这里可以加上斜向运动的处理，
        // rb.velocity = new Vector2(inputDirection.x *speed * Time.deltaTime, inputDirection.y * speed * Time.deltaTime); 
        //因为是横版卷轴，所以这里只用x轴的速度。
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //设置Rigidbody2D的Velocity属性,//不需要Y轴，所以保持原有的Y轴速度


        //人物翻转
        int faceDir = (int)inputDirection.x;
        if (inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

    }

    private void Jump(InputAction.CallbackContext context)
    {
             if(physicsCheck.isGround==true)
             {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //添加一个向上方向的力，用于跳跃
             }
    }
 
}
