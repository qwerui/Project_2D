using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyClass
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyInit();
        CreateItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() 
    {
        if(!isDead)
        {
            Move();
        }    
    }
    protected override void EnemyInit()
    {
        hp = 10;
        atk = 100;
        def = 0;
        moveSpeed = 3.0f;
        experience = 1;
    }
    protected override void Attack()
    {

    }
    protected override void Think()
    {

    }
    protected override void Move()
    {
        rigid.velocity= new Vector2(moveSpeed, 0);
        ani.SetBool("Run", true);
    }
    protected override void Chase()
    {

    }
    protected override IEnumerator PosDiff()
    {
        yield return null;
    }
}
