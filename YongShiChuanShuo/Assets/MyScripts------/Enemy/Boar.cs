using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy
{
    public override void Move()
    {
        base.Move();//继承父类的移动方法
        anim.SetBool("Walk", true);//是否死亡

    }
}


