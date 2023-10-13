using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : EnemyClass
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
        hp = 12 * data.stage + 10;
        atk = 2 * data.stage + 10;
        def = (int)(data.stage/5);
        experience = data.stage * 7;
    }
}
