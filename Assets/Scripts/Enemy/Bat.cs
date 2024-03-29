using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyClass
{
    float posDiff_y;
    int nextMove_y;

    void Start()
    {
        EnemyInit();
        StartCoroutine("PosDiff");
    }
    private void Update() {

    }
    private void FixedUpdate() 
    {
        if(!isDead)
        {
            if(!isHit)
            {
                if(SetOnChase(6.0f))
                    Chase();
                else
                    Move();
            }
        }    
    }

    protected override void Attack()
    {

    }
    protected override void Think()
    {

    }
    protected override void Move()
    {
        rigid.velocity= Vector2.zero;
    }
    protected override void Chase()
    {
        isMoving = true;
        if(posDiff<0)
            nextMove = 1;
        else
            nextMove = -1;
        transform.localScale = new Vector3(nextMove*0.7f,0.7f,1);
        rigid.velocity= new Vector2(moveSpeed*nextMove, -posDiff_y+2);
        ani.SetBool("Run", isMoving);
    }
    protected override IEnumerator PosDiff()
    {
        while(true)
        {
            posDiff = transform.position.x - player.transform.position.x;
            posDiff_y = transform.position.y - player.transform.position.y;
            yield return new WaitForSeconds(1f);
        }
    }
    protected override void StatusInit()
    {
        hp = 3 * data.stage + 5;
        def = (int)(data.stage/5);
        if(gameObject.name.Contains("Normal"))
        {
            atk = 5 * (int)(data.stage / 3) + 5;
            experience = data.stage * 12;
        }
        else if(gameObject.name.Contains("Strong"))
        {
            atk = 3 * data.stage + 5;
            experience = data.stage * 18;
        }
        else
        {
            atk = 5  * (int)(data.stage / 5) + 5;
            experience = data.stage * 8;
        }
    }
}
