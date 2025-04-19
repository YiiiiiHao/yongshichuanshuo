using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;// 攻击伤害
    public float attackRange;// 攻击范围
    public float attackRate;// 攻击频率

    private void OnTriggerStay2D(Collider2D collision)
    {
        //通过collision访问被攻击的对象，强调：是被攻击的对象，不是攻击者
        //如：野猪挂载此脚本，碰到玩家，则调用此脚本的OnTriggerStay2D函数，并传入玩家的Collider2D对象
        //访问被攻击者的Character脚本，获取其当前血量，并减去攻击伤害，等于当前血量返还给被攻击者
        //被攻击者血量100，攻击伤害5，攻击者攻击，被攻击者血量减少5，等于95
        // collision.GetComponent<Character>().currentHealth -= damage;
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
