using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : EnemyClass
{
    private void Awake() 
    {
        Think(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        EnemyInit();
        StartCoroutine("PosDiff");
    }

    private void FixedUpdate() 
    {
        if(!isDead)
        {
            PlatformCheck();
            WallCheck();
            if(!isHit)
            {
                if(SetOnChase(4.0f))
                    Chase();
                else
                    Move();
            }
        }
    }

    protected override void Attack()
    {

    }
    
    protected override void Move()
    {
        if(nextMove != 0)
        {
            isMoving = true;
            transform.localScale = new Vector3(nextMove*transform.localScale.y,transform.localScale.y,1);
        }
        else
        {
            isMoving=false;
        }
        rigid.velocity= new Vector2(moveSpeed*nextMove, 0.001f);
        ani.SetBool("Run", isMoving);
    }
    protected override void Think()
    {
        nextMove = Random.Range(-1,2);
        float time = Random.Range(2f, 5f);
        Invoke("Think", time);
    }
    protected override void Chase()
    {
        isMoving = true;
        if(posDiff<0)
            nextMove = 1;
        else
            nextMove = -1;
        transform.localScale = new Vector3(nextMove*transform.localScale.y,transform.localScale.y,1);
        rigid.velocity= new Vector2(moveSpeed*nextMove, 0);
        ani.SetBool("Run", isMoving);
    }
    protected override IEnumerator PosDiff()
    {
        while(true)
        {
            posDiff = transform.position.x - player.transform.position.x;
            yield return new WaitForSeconds(1f);
        }
    }
    protected override void StatusInit()
    {
        if(gameObject.name.Contains("Normal"))
        {
            hp =  7 * data.stage + 8;
            atk = 5 * (int)(data.stage / 3) + 10;
            def = (int)(data.stage/8);
            experience = data.stage * 5;
        }
        else if(gameObject.name.Contains("Strong"))
        {
            hp = 10 * data.stage + 5;
            atk = 3 * data.stage + 12;
            def = (int)(data.stage/5);
            experience = data.stage * 10;
        }
        else if(gameObject.name.Contains("Tutorial"))
        {
            return;
        }
        else
        {
            hp = 5 * data.stage + 5;
            atk = 5  * (int)(data.stage / 5) + 10;
            def = (int)(data.stage/10);
            experience = data.stage * 3;
        }
    }
} 
