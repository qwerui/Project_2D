using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : EnemyClass
{
    private void Awake() {
        Think();
        StartCoroutine("PosDiff");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate() {
        PlatformCheck();
        WallCheck();
        if(SetOnChase(4.0f))
            Chase();
        else
            Move();
    }
    public override void Attack()
    {

    }
    
    public override void Move()
    {
        if(nextMove != 0)
        {
            isMoving = true;
            transform.localScale = new Vector3(nextMove,1,1);
        }
        else
        {
            isMoving=false;
        }
        rigid.velocity= new Vector2(moveSpeed*nextMove, 0);
        ani.SetBool("Run", isMoving);
    }
    public override void Think()
    {
        nextMove = Random.Range(-1,2);
        float time = Random.Range(2f, 5f);
        Invoke("Think", time);
    }
    public override void Chase()
    {
        isMoving = true;
        if(posDiff<0)
            nextMove = 1;
        else
            nextMove = -1;
        transform.localScale = new Vector3(nextMove,1,1);
        rigid.velocity= new Vector2(moveSpeed*nextMove, 0);
        ani.SetBool("Run", isMoving);
    }
    public override IEnumerator PosDiff()
    {
        while(true)
        {
            posDiff = transform.position.x - player.transform.position.x;
            yield return new WaitForSeconds(1f);
        }
    }
} 
