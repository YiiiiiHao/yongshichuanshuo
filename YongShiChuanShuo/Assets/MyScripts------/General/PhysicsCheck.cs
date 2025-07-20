using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    [Header("检测参数")]
    public bool manual;//是否手动控制
    public Vector2 bottomOffset;//位移差值
    public Vector2 LeftOffset;//左墙检测偏移
    public Vector2 RightOffset;//右墙检测偏移
    public float checkRadius = 3f;//检测半径
    public LayerMask groundLayer;//地面层
    [Header("状态")]
    public bool isGround;//是否站在地面
    public bool touchLeftWall;//是否碰撞到左墙
    public bool touchRightWall;//是否碰撞到右墙
    void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            ///自动计算位移差值
            RightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            LeftOffset = new Vector2(-RightOffset.x, RightOffset.y);
        }
    }
    void Update()
    {
        Check();
    }

    void Check()
    {
        //检测碰撞效果已完成，下面的是要绘制检测范围
        // isGround = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

        //检测是否站在地面,站在地面则isGround为true，否则为false
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);//检测碰撞

        //检测是否碰撞到左墙
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffset, checkRadius, groundLayer);//检测碰撞
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + RightOffset, checkRadius, groundLayer);//检测碰撞
    }

    private void OnDrawGizmosSelected()//绘制碰撞范围
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + RightOffset, checkRadius);
    }

}
