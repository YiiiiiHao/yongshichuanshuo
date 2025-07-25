using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{ 
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth ;//当前生命值
    [Header("受伤无敌")]
    public float invulnerableDuration ;//受伤无敌持续时间
    private float invulnerableCounter ;//受伤无敌计时器
    public bool invulnerable ;//受伤无敌状态
    public bool isDead ;//死亡状态
    public UnityEvent<Transform> onDakeDamage;//受到伤害事件
    public UnityEvent onDie;//死亡事件
    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
    
    void Update()
    { 
        if(invulnerable)//触发invulnerable状态,受伤无敌状态
        {
            //invulnerableCounter在Unity中设置数值，减去时间，直到为0，则恢复正常状态
         invulnerableCounter -= Time.deltaTime;//受到伤害后受伤无敌计时器开始
         if(invulnerableCounter<=0)
         {
             invulnerable = false;
         }
        }
    }

    public void TakeDamage(Attack attack)//接受伤害，受到攻击。这里的Attack是自定义的攻击类，Attack脚本。
    {
        if(invulnerable==true)//如果受伤无敌状态，则不受到伤害
        {
            return;
        }
        if (currentHealth - attack.damage > 0)//如果受到伤害后生命值大于0，则扣除生命值，触发受伤无敌状态
        {
            currentHealth -= attack.damage;
            TriggerInvulnerable();//触发受伤无敌状态
            //触发受伤事件
            onDakeDamage?.Invoke(attack.transform);//攻击者的transform

        }
        else if (currentHealth - attack.damage <= 0)//如果受到伤害后生命值等于或小于0，则死亡，销毁对象
        {
            currentHealth = 0;
            isDead = true;
            onDie?.Invoke();
            // Destroy(gameObject);
        }
    }

    private void TriggerInvulnerable()//触发受伤无敌状态
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;//受伤无敌计时器开始
        }
    }
}
