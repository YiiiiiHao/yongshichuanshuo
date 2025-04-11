using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;//用于读取PlayerInputControl的MOve里的Vector2值 -1 ~ 1
    //要用Vector2的值来驱动RIgidbody2D的Velocity属性。
    public float speed = 5f;  //速度
    public Rigidbody2D rb;//用于驱动Rigidbody2D的Velocity属性

    private void Awake()
    {
        inputControl = new PlayerInputControl(); //实例化PlayerInputControl类
        // rb = GetComponent<Rigidbody2D>(); //获取Rigidbody2D组件
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
    }
    public void Move()
    {
        //如果是星露谷物语那种，可以用下面这行代码，并将RIgidbody2D的gravity属性设置为0，这里可以加上斜向运动的处理，
        // rb.velocity = new Vector2(inputDirection.x *speed * Time.deltaTime, inputDirection.y * speed * Time.deltaTime); 
        //因为是横版卷轴，所以这里只用x轴的速度。
        rb.velocity = new Vector2(inputDirection.x *speed * Time.deltaTime,rb.velocity.y); //设置Rigidbody2D的Velocity属性,//不需要Y轴，所以保持原有的Y轴速度


        //人物翻转
        int faceDir= (int)inputDirection.x;
        if(inputDirection.x>0)
        {
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
        else if(inputDirection.x<0)
        {
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        }
        
       
    }
}
