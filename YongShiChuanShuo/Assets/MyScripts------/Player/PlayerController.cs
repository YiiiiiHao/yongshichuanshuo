using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //获取
    private CapsuleCollider2D capsuleCollider2D;//椭圆碰撞器
    public PlayerInputControl inputControl;//获取角色控制器
    public PhysicsCheck physicsCheck;//获取角色的物理检测组件
    private PlayerAnimation playerAnimation;//获取角色动画组件

    //要用Vector2的值来驱动RIgidbody2D的Velocity属性。
    public Vector2 inputDirection;//用于读取PlayerInputControl的MOve里的Vector2值 在Unity显示为-1 ~ 1
    public Rigidbody2D rb;//用于驱动Rigidbody2D的Velocity属性
    [Header("基本参数")]
    public float speed = 5f;  //速度
    public float runSpeed; //跑步速度
    public float walkSpeed => speed * 0.5f; //走路速度
    public float jumpForce = 10f;
    private float offstY;//记录站立时offstY的值
    private float sizeY;//记录站立时sizeY的值
    public float crouchOffstY;//角色下蹲时的offstY
    public float crouchSizeY;//角色下蹲时的sizeY
    public float hutForce;
    [Header("材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("状态")]
    public bool isCrouch;//角色是否处于下蹲状态
    public bool isHurt;//角色是否受伤
    public bool isDead;//角色是否死亡
    public bool isAttack;//角色是否攻击


    //Jump

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>(); //获取PlayerAnimation组件
        physicsCheck = GetComponent<PhysicsCheck>(); //获取PhysicsCheck组件
        capsuleCollider2D = GetComponent<CapsuleCollider2D>(); //获取CapsuleCollider2D组件
        offstY = capsuleCollider2D.offset.y;//记录站立时offstY的值,椭圆碰撞器 Y 的偏移值
        sizeY = capsuleCollider2D.size.y;//记录站立时sizeY的值，椭圆碰撞器的 Y 轴大小

        inputControl = new PlayerInputControl(); //实例化PlayerInputControl类
        // rb = GetComponent<Rigidbody2D>(); //获取Rigidbody2D组件

        //staerted:按下按键瞬间则启用
        //按下空格键和右键（在PlayerInputControl里设置）时，调用Jump方法
        inputControl.GamePlay.Jump.started += Jump;


        #region 强制走路
        //****************************************************强制走路****************************************************
        runSpeed = speed; //跑步速度一开始就确定下来
        inputControl.GamePlay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
            {
                // Debug.Log(speed);
                speed = walkSpeed;
            }
        };
        inputControl.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                //   Debug.Log(speed);
                speed = runSpeed;
            }
        };
        //****************************************************强制走路****************************************************
        #endregion



        //***************攻击**************
        inputControl.GamePlay.Attack.started += playerAttack; //添加攻击事件监听


        //****************攻击**************



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
        material(); //设置角色的材质
    }

    void FixedUpdate()//方法用于驱动Rigidbody2D的Velocity属性
    {
        if (!isHurt && !isAttack)
        {
            Move();
        }
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log(collision.name);
    // }
    public void Move()
    {
        //如果是星露谷物语那种，可以用下面这行代码，并将RIgidbody2D的gravity属性设置为0，这里可以加上斜向运动的处理，
        // rb.velocity = new Vector2(inputDirection.x *speed * Time.deltaTime, inputDirection.y * speed * Time.deltaTime); 
        //因为是横版卷轴，所以这里只用x轴的速度。
        if (isCrouch == false)//只有不是下蹲状态才能移动
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //设置Rigidbody2D的Velocity属性,//不需要Y轴，所以保持原有的Y轴速度
        }
        if (isCrouch == true)//下蹲状态则不能移动，speed为0
        {
            rb.velocity = new Vector2(inputDirection.x * 0 * Time.deltaTime, rb.velocity.y); //设置Rigidbody2D的Velocity属性,//不需要Y轴，所以保持原有的Y轴速度
        }


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

        //下蹲
        //inputDirection.y < -0.1的同时physicsCheck.isGround == true，说明角色在地面上并且下蹲
        isCrouch = inputDirection.y < -0.1 && physicsCheck.isGround == true;

        if (isCrouch == true)
        {
            capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x, crouchOffstY); //设置CapsuleCollider2D的offset属性，使角色下蹲
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x, crouchSizeY); //设置CapsuleCollider2D的offset属性，使角色下蹲

        }
        else if (isCrouch == false)
        {
            capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x, offstY); //角色站立式参数是多少就是多少
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x, sizeY); //角色站立式参数是多少就是多少

        }

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround == true)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //添加一个向上方向的力，用于跳跃
        }
    }

    private void playerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack(); //播放攻击动画
        isAttack = true;
    }


    #region UnityEvent

    
    public void GetHurt(Transform attake)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;//清空角色刚体速度
        Vector2 dir = new Vector2((transform.position.x - attake.position.x), 0).normalized;//normalized方法用于将向量标准化，使其长度为1
        rb.AddForce(dir * hutForce, ForceMode2D.Impulse); //添加一个反方向的力，用于受伤
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable(); //禁用PlayerInputControl类,玩家不再被控制

    }
    #endregion

    void material()
    {
        capsuleCollider2D.sharedMaterial = physicsCheck.isGround ? normal : wall; //设置角色的材质
    }

}
