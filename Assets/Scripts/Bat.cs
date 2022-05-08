using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyClass
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() {
        Move();    
    }
    public override void Attack()
    {

    }
    public override void Move()
    {
        rigid.velocity= new Vector2(moveSpeed, 0);
        ani.SetBool("Run", true);
    }
}
