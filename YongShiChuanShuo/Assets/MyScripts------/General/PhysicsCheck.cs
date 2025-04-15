using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{       
    [Header("检测参数")]
    public Vector2 bottomOffset;//位移差值
    public float checkRadius = 3f;//检测半径
    public LayerMask groundLayer;//地面层
    [Header("状态")]
    public bool isGround;//是否站在地面
    void Update()
    {
        Check();
    }

    void Check()
    {
        //检测碰撞效果已完成，下面的是要绘制检测范围
        // isGround = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

        //检测是否站在地面,站在地面则isGround为true，否则为false
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRadius, groundLayer);//检测碰撞
    }

    private void OnDrawGizmosSelected()//绘制碰撞范围
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset,checkRadius);
    }

}
