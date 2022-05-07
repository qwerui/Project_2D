using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass
{
    public int hp;
    public int atk;
    public int def;
    public float moveSpeed;
    public Animator ani;

    public abstract void Move();
    public abstract void Attack();
    public void Hit()
    {
        ani.SetTrigger("Hit");
    }
}
